using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DataAccess.Entities;
using DataAccess.Data;

namespace MusicCollection_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAlbumController : ControllerBase
    {
        private readonly MusicColectionsDbContext _context;

        public UserAlbumController(MusicColectionsDbContext context)
        {
            _context = context;
        }

        // Отримати всі записи користувачів
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserAlbum>>> GetAll()
        {
            var userAlbums = await _context.UserAlbums
                .Include(ua => ua.Album)
                .ToListAsync();
            return Ok(userAlbums);
        }

        // Отримати запис за ID
        [HttpGet("{id}")]
        public async Task<ActionResult<UserAlbum>> GetById(int id)
        {
            var userAlbum = await _context.UserAlbums
                .Include(ua => ua.Album)
                .FirstOrDefaultAsync(ua => ua.UserAlbumId == id);

            if (userAlbum == null)
                return NotFound();

            return Ok(userAlbum);
        }

        [HttpPost]
        [HttpPost]
        public async Task<ActionResult<UserAlbum>> Create([FromBody] UserAlbum userAlbum)
        {
            if (userAlbum == null)
            {
                return BadRequest("UserAlbum data is required.");
            }

           
            if (string.IsNullOrEmpty(userAlbum.Status))
            {
                userAlbum.Status = "Unknown"; // дефолтне значення
            }

            // Перевірка альбом ID
            var album = await _context.Albums.FindAsync(userAlbum.AlbumId);
            if (album == null)
            {
                return BadRequest("Album not found.");
            }

            // Призначаємо UserAlbum
            userAlbum.Album = album;

            // Додаємо новий запис
            _context.UserAlbums.Add(userAlbum);
            await _context.SaveChangesAsync();

            // Повертаємо 
            return CreatedAtAction(nameof(GetById), new { id = userAlbum.UserAlbumId }, userAlbum);
        }





        // Оновити запис
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UserAlbum updatedUserAlbum)
        {
            if (id != updatedUserAlbum.UserAlbumId)
                return BadRequest();

            // Якщо пусто, можно не міняти
            if (string.IsNullOrEmpty(updatedUserAlbum.Status))
            {
                updatedUserAlbum.Status = "Unknown"; //дефолтне значення
            }

            _context.Entry(updatedUserAlbum).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.UserAlbums.Any(ua => ua.UserAlbumId == id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // Видалити запис
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userAlbum = await _context.UserAlbums.FindAsync(id);
            if (userAlbum == null)
                return NotFound();

            _context.UserAlbums.Remove(userAlbum);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
