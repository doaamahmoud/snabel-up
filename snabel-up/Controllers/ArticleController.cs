using snabel_up.DTO;
using snabel_up.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace snabel_up.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private new List<string> allowExxtention = new List<string> { ".png", ".jpg" };

        public ArticleController(ApplicationDbContext context)
        {
            _context = context;
        }
       
        [HttpGet("GetAllArticles")]
        public async Task<ActionResult<IEnumerable<Article>>> GetArticlesAsync()
        {
          List<Article> articles = await _context.articles.ToListAsync();
            return Ok(articles);
        }
        [HttpGet("GetArticleByID/{id}")]
        public async Task<ActionResult<Article>> GetArticleByIDAsync(int id)
        {
            var article = await _context.articles.FindAsync(id);

            if (article == null)
            {
                return NotFound($"No Article was found with ID {id}");
            }

            return Ok(article);
        }
       
        [HttpPost("AddArticle")]
        public async Task<ActionResult<Article>> CreateProductAsync([FromForm] ArticleDto art)
        {
            if (art.Image == null) return BadRequest("Image Is Required ");
            if (!allowExxtention.Contains(Path.GetExtension(art.Image.FileName).ToLower()))
                return BadRequest("Onl .png or .jpg images are allow ");

            using var dataStream = new MemoryStream();
            await art.Image.CopyToAsync(dataStream);
            var article = new Article
            {
                Name = art.Name,
                Image = dataStream.ToArray(),
                Description = art.Description
            };
            await _context.AddAsync(article);
            _context.SaveChanges();

            return Ok(article);

        }


        [HttpPut("UpdateArticle/{id}")]
        public async Task<IActionResult> UpdateArticleAsync(int id, [FromForm] ArticleDto art)
        {
            var article = await _context.articles.FindAsync(id);
            if (article == null)
                return NotFound($"No Article was found with ID {id}");
            if (art.Image != null)
            {
                if (!allowExxtention.Contains(Path.GetExtension(art.Image.FileName).ToLower()))
                    return BadRequest("Onl .png or .jpg images are allow ");
                using var dataStream = new MemoryStream();
                await art.Image.CopyToAsync(dataStream);
                article.Image = dataStream.ToArray();
            }
            article.Name = art.Name;
            article.Description = art.Description;
            await _context.SaveChangesAsync();
            return Ok(article);
        }

    

        [HttpDelete("DeleteArticle/{id}")]
        public async Task<IActionResult> DeleteArticleAsync(int id)
        {
            var article = await _context.articles.FindAsync(id);
            if (article == null)
            {
                return NotFound();
            }

            _context.Remove(article);
            await _context.SaveChangesAsync();

            return Ok(article);
        }

    }
}
