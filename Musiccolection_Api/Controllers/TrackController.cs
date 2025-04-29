using DataAccess.Data;
using DataAccess.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Musiccolection_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrackController : ControllerBase
    {
        private readonly MusicColectionsDbContext _context;

        public TrackController(MusicColectionsDbContext context)
        {
            _context = context;
        }

       
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Track>>> GetTracks()
        {
            var tracks = await _context.Tracks
                                       .Include(t => t.Album)
                                       .ToListAsync();
            return Ok(tracks);
        }

       
        [HttpGet("{id}")]
        public async Task<ActionResult<Track>> GetTrack(int id)
        {
            var track = await _context.Tracks
                                       .Include(t => t.Album)
                                       .FirstOrDefaultAsync(t => t.TrackId == id);

            if (track == null)
            {
                return NotFound();
            }

            return Ok(track);
        }


        [HttpPost]
        public async Task<ActionResult<Track>> CreateTrack([FromBody] Track track)
        {
            if (track == null)
            {
                return BadRequest("Track cannot be null.");
            }

            // Перевірка 
            if (track.AlbumId.HasValue)
            {
                var album = await _context.Albums.FindAsync(track.AlbumId.Value);
                if (album == null)
                {
                    return BadRequest("Album not found.");
                }
            }

            //  "Untitled Track"
            if (string.IsNullOrEmpty(track.Title))
            {
                track.Title = "Untitled Track";
            }

            // Додавання
            _context.Tracks.Add(track);
            await _context.SaveChangesAsync();

            // Повернення 
            return CreatedAtAction("GetTrack", new { id = track.TrackId }, track);
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTrack(int id, [FromBody] Track track)
        {
            if (id != track.TrackId)
            {
                return BadRequest("Track ID mismatch.");
            }

            
            if (track.AlbumId != 0)
            {
                var album = await _context.Albums.FindAsync(track.AlbumId);
                if (album == null)
                {
                    return BadRequest("Album not found.");
                }
            }

            
            if (string.IsNullOrEmpty(track.Title))
            {
                track.Title = "Untitled Track";
            }

            _context.Entry(track).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TrackExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

      
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrack(int id)
        {
            var track = await _context.Tracks.FindAsync(id);
            if (track == null)
            {
                return NotFound();
            }

            _context.Tracks.Remove(track);
            await _context.SaveChangesAsync();

            return NoContent();
        }

      
        private bool TrackExists(int id)
        {
            return _context.Tracks.Any(e => e.TrackId == id);
        }
    }
}
