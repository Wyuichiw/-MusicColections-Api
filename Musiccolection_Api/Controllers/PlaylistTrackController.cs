using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DataAccess.Entities;
using DataAccess.Data;

namespace MusicCollection_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlaylistTrackController : ControllerBase
    {
        private readonly MusicColectionsDbContext _context;

        public PlaylistTrackController(MusicColectionsDbContext context)
        {
            _context = context;
        }

        // Отримати всі записи про треки в плейлистах
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlaylistTrack>>> GetAll()
        {
            var playlistTracks = await _context.PlaylistTracks
                .Include(pt => pt.Playlist)
                .Include(pt => pt.Track)
                .ToListAsync();
            return Ok(playlistTracks);
        }

        // Отримати записи по ID плейлиста
        [HttpGet("playlist/{playlistId}")]
        public async Task<ActionResult<IEnumerable<PlaylistTrack>>> GetByPlaylistId(int playlistId)
        {
            var playlistTracks = await _context.PlaylistTracks
                .Where(pt => pt.PlaylistId == playlistId)
                .Include(pt => pt.Playlist)
                .Include(pt => pt.Track)
                .ToListAsync();

            if (!playlistTracks.Any())
                return NotFound("No tracks found for this playlist.");

            return Ok(playlistTracks);
        }

        // Отримати записи по ID трека
        [HttpGet("track/{trackId}")]
        public async Task<ActionResult<IEnumerable<PlaylistTrack>>> GetByTrackId(int trackId)
        {
            var playlistTracks = await _context.PlaylistTracks
                .Where(pt => pt.TrackId == trackId)
                .Include(pt => pt.Playlist)
                .Include(pt => pt.Track)
                .ToListAsync();

            if (!playlistTracks.Any())
                return NotFound("No playlists found for this track.");

            return Ok(playlistTracks);
        }

        // Отримати записи по обом ID плейлиста і трека
        [HttpGet("{playlistId}/{trackId}")]
        public async Task<ActionResult<PlaylistTrack>> GetByIds(int playlistId, int trackId)
        {
            var playlistTrack = await _context.PlaylistTracks
                .Include(pt => pt.Playlist)
                .Include(pt => pt.Track)
                .FirstOrDefaultAsync(pt => pt.PlaylistId == playlistId && pt.TrackId == trackId);

            if (playlistTrack == null)
                return NotFound("Track not found in this playlist.");

            return Ok(playlistTrack);
        }

        // Додати трек до плейлиста
        [HttpPost]
        public async Task<ActionResult<PlaylistTrack>> Create([FromBody] PlaylistTrack playlistTrack)
        {
            if (playlistTrack == null)
                return BadRequest("PlaylistTrack data is required.");

            // Перевірка на наявність запису
            var existingPlaylistTrack = await _context.PlaylistTracks
                .AnyAsync(pt => pt.PlaylistId == playlistTrack.PlaylistId && pt.TrackId == playlistTrack.TrackId);
            if (existingPlaylistTrack)
                return Conflict("This track is already in the playlist.");

            // Перевірка на наявність плейлиста та трека
            var playlistExists = await _context.Playlists.AnyAsync(p => p.PlaylistId == playlistTrack.PlaylistId);
            var trackExists = await _context.Tracks.AnyAsync(t => t.TrackId == playlistTrack.TrackId);

            if (!playlistExists)
                return BadRequest("Playlist does not exist.");
            if (!trackExists)
                return BadRequest("Track does not exist.");

            
            playlistTrack.Playlist = null; // Уникаємо передачі значення для PlaylistId
            playlistTrack.Track = null; // Уникаємо передачі значення для TrackId

            _context.PlaylistTracks.Add(playlistTrack);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetByIds), new { playlistId = playlistTrack.PlaylistId, trackId = playlistTrack.TrackId }, playlistTrack);
        }


        // Видалити трек з плейлиста
        [HttpDelete("{playlistId}/{trackId}")]
        public async Task<IActionResult> Delete(int playlistId, int trackId)
        {
            var playlistTrack = await _context.PlaylistTracks
                .FirstOrDefaultAsync(pt => pt.PlaylistId == playlistId && pt.TrackId == trackId);

            if (playlistTrack == null)
                return NotFound();

            _context.PlaylistTracks.Remove(playlistTrack);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
