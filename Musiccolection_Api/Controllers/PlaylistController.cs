using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DataAccess.Entities;
using DataAccess.Data;

namespace MusicCollection_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlaylistController : ControllerBase
    {
        private readonly MusicColectionsDbContext _context;

        public PlaylistController(MusicColectionsDbContext context)
        {
            _context = context;
        }

        // Отримати всі плейлисти
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Playlist>>> GetAll()
        {
            var playlists = await _context.Playlists.ToListAsync();
            return Ok(playlists);
        }

        // Отримати плейлист за ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Playlist>> GetById(int id)
        {
            var playlist = await _context.Playlists.FindAsync(id);

            if (playlist == null)
                return NotFound();

            return Ok(playlist);
        }

        // Створити новий плейлист
        [HttpPost]
        public async Task<ActionResult<Playlist>> Create([FromBody] Playlist playlist)
        {
            if (playlist == null)
                return BadRequest("Playlist data is required.");

            if (string.IsNullOrWhiteSpace(playlist.Title))
                return BadRequest("Playlist title is required.");

            if (string.IsNullOrWhiteSpace(playlist.Author))
                return BadRequest("Playlist author is required.");

            playlist.CreatedDate = DateTime.UtcNow;  // Встановлюємо поточну дату створення

            _context.Playlists.Add(playlist);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = playlist.PlaylistId }, playlist);
        }

        // Оновити плейлист
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Playlist playlist)
        {
            if (id != playlist.PlaylistId)
                return BadRequest("ID mismatch.");

            _context.Entry(playlist).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Playlists.Any(p => p.PlaylistId == id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // Видалити плейлист
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var playlist = await _context.Playlists.FindAsync(id);
            if (playlist == null)
                return NotFound();

            _context.Playlists.Remove(playlist);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
