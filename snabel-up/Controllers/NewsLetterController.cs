using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using snabel_up.Models;

namespace snabel_up.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsLetterController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public NewsLetterController(ApplicationDbContext context)
        {
            _context = context;
        }

      
        [HttpGet("GetAllNewsLetters")]
        public async Task<ActionResult<IEnumerable<NewsLetter>>> GetNewsLettersAsync()
        {
            List<NewsLetter> newsLetters = await _context.newsLetters.ToListAsync();
            return Ok(newsLetters);
        }


        [HttpGet("GetNewsLetterById/{id}")]
        public async Task<ActionResult<NewsLetter>> GetNewsLetterAsync(int id)
        {
            var newsLetter = await _context.newsLetters.FindAsync(id);

            if (newsLetter == null)
            {
                return NotFound($"No NewsLetter was found with ID {id}");
            }

            return Ok(newsLetter);
        }

       
        [HttpPut("UpdateNewsLetter/{id}")]
        public async Task<IActionResult> UpdateNewsLetterAsync(int id, [FromBody] NewsLetter newsLetter)
        {
            var news = await _context.newsLetters.FindAsync(id);
            if (news == null)
            {
                return NotFound($"No NewsLetter was found with ID {id}");
            }
            news.Name = newsLetter.Name;
            news.Email = newsLetter.Email;
            await _context.SaveChangesAsync();
            return Ok(news);
        }

  
        [HttpPost("AddNewsLetter")]
        public async Task<ActionResult<NewsLetter>> CreateNewsLetterAsync(NewsLetter news)
        {
            var Newsletter = new NewsLetter {
                Name = news.Name,
                Email=news.Email
                   };
            await _context.AddAsync(Newsletter);
            _context.SaveChanges();

            return Ok(Newsletter);
        }

        [HttpDelete("DeleteNewsLetterByID/{id}")]
        public async Task<IActionResult> DeleteNewsLetterAsync(int id)
        {
            var newsLetter = await _context.newsLetters.FindAsync(id);
            if (newsLetter == null)
            {
                return NotFound();
            }

            _context.Remove(newsLetter);
            await _context.SaveChangesAsync();

            return Ok(newsLetter);
        }
    }
}
