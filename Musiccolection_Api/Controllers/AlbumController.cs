using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DataAccess.Entities;
using DataAccess.Data;

namespace MusicCollection_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlbumController : ControllerBase
    {
        private readonly MusicColectionsDbContext _context;

        public AlbumController(MusicColectionsDbContext context)
        {
            _context = context;
        }

        // всі альбоми
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Album>>> GetAll()
        {
            var albums = await _context.Albums
                .Include(a => a.Artist)  // Завантаження артиста 
                .Include(a => a.Tracks)  // Завантаження треків 
                .ToListAsync();
            return Ok(albums);
        }

        // альбом за ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Album>> GetById(int id)
        {
            var album = await _context.Albums
                .Include(a => a.Artist)
                .Include(a => a.Tracks)
                .FirstOrDefaultAsync(a => a.AlbumId == id);

            if (album == null)
                return NotFound();

            return Ok(album);
        }

        // новий альбом
        [HttpPost]
        public async Task<ActionResult<Album>> Create([FromBody] Album album)
        {
            if (album == null)
                return BadRequest("Album data is required.");

            if (string.IsNullOrWhiteSpace(album.Title))
                return BadRequest("Album title is required.");

            _context.Albums.Add(album);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = album.AlbumId }, album);
        }

        // Оновити альбом
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Album album)
        {
            if (id != album.AlbumId)
                return BadRequest("ID mismatch.");

            _context.Entry(album).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Albums.Any(a => a.AlbumId == id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // Видалити альбом
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var album = await _context.Albums.FindAsync(id);
            if (album == null)
                return NotFound();

            _context.Albums.Remove(album);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
