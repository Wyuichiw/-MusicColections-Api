using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DataAccess.Entities;
using DataAccess.Data;

namespace MusicCollection_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly MusicColectionsDbContext _context;

        public ReviewController(MusicColectionsDbContext context)
        {
            _context = context;
        }

        // Отримати всі відгуки для альбому
        [HttpGet("album/{albumId}")]
        public async Task<ActionResult<IEnumerable<Review>>> GetReviewsByAlbum(int albumId)
        {
            var reviews = await _context.Reviews
                .Where(r => r.AlbumId == albumId)
                .Include(r => r.Album)
                .ToListAsync();

            if (reviews == null || reviews.Count == 0)
                return NotFound("No reviews found for this album.");

            return Ok(reviews);
        }

        // Отримати конкретний відгук за ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Review>> GetReviewById(int id)
        {
            var review = await _context.Reviews
                .Include(r => r.Album)
                .FirstOrDefaultAsync(r => r.ReviewId == id);

            if (review == null)
                return NotFound();

            return Ok(review);
        }

        // Створити новий відгук
        [HttpPost]
        public async Task<ActionResult<Review>> CreateReview([FromBody] Review review)
        {
            if (review == null)
                return BadRequest("Review data is required.");

            if (review.Rating < 1 || review.Rating > 5)
                return BadRequest("Rating must be between 1 and 5.");

            // Перевірка, чи існує альбом із таким ID
            var album = await _context.Albums.FindAsync(review.AlbumId);
            if (album == null)
                return BadRequest("The album with the given ID does not exist.");

            // Прив'язка альбому до відгуку
            review.Album = album;

            // Додавання відгуку до бази
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetReviewById), new { id = review.ReviewId }, review);
        }





        // Оновити відгук
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReview(int id, [FromBody] Review review)
        {
            if (review == null || review.ReviewId != id)
                return BadRequest("Review data is invalid.");

            var existingReview = await _context.Reviews
                .Include(r => r.Album)  
                .ThenInclude(a => a.Artist)  
                .FirstOrDefaultAsync(r => r.ReviewId == id);

            if (existingReview == null)
                return NotFound("Review not found.");

            // Оновлюємо дані рецензії
            existingReview.UserName = review.UserName;
            existingReview.Rating = review.Rating;
            existingReview.Comment = review.Comment;

            // Оновлюємо альбом, якщо він змінився
            if (review.AlbumId != existingReview.AlbumId)
            {
                // Перевірка на існування альбому
                var album = await _context.Albums
                    .Include(a => a.Artist)  
                    .FirstOrDefaultAsync(a => a.AlbumId == review.AlbumId);

                if (album == null)
                    return NotFound("Album not found.");

                existingReview.AlbumId = review.AlbumId;
                existingReview.Album = album;  
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }


        // Видалити відгук
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            var review = await _context.Reviews.FindAsync(id);

            if (review == null)
                return NotFound();

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
