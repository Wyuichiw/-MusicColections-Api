using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DataAccess.Entities;
using DataAccess.Data;

namespace MusicCollection_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtistController : ControllerBase
    {
        private readonly MusicColectionsDbContext _context;

        public ArtistController(MusicColectionsDbContext context)
        {
            _context = context;
        }

        // Отримати всіх артистів
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Artist>>> GetAll()
        {
            var artists = await _context.Artists
                .Include(a => a.Albums)  // Завантаження альбомів для кожного артиста
                .ToListAsync();
            return Ok(artists);
        }

        // Отримати артиста за ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Artist>> GetById(int id)
        {
            var artist = await _context.Artists
                .Include(a => a.Albums)  // Завантаження альбомів для артиста
                .FirstOrDefaultAsync(a => a.ArtistId == id);

            if (artist == null)
                return NotFound();

            return Ok(artist);
        }

        // Створити нового артиста
        [HttpPost]
        public async Task<ActionResult<Artist>> Create([FromBody] Artist artist)
        {
            if (artist == null)
                return BadRequest("Artist data is required.");

            if (string.IsNullOrWhiteSpace(artist.Name))
                return BadRequest("Artist name is required.");

            _context.Artists.Add(artist);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = artist.ArtistId }, artist);
        }

        // Оновити артиста
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Artist artist)
        {
            if (id != artist.ArtistId)
                return BadRequest("ID mismatch.");

            _context.Entry(artist).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Artists.Any(a => a.ArtistId == id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // Видалити артиста
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var artist = await _context.Artists.FindAsync(id);
            if (artist == null)
                return NotFound();

            _context.Artists.Remove(artist);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
