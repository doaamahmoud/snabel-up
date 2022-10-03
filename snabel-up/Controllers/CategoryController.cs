using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using snabel_up.Models;

namespace snabel_up.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Categories
        [HttpGet("GetAllCategories")]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategoriesAsync()
        {
            List<Category> Categories = await _context.categories.ToListAsync();
            return Ok(Categories);
        }

        // GET: api/Category/5
        [HttpGet("GetCategoryById/{id}")]
        public async Task<ActionResult<Category>> GetCategoryAsync(int id)
        {
            var category = await _context.categories.FindAsync(id);

            if (category == null)
            {
                return NotFound($"No category was found with ID {id}");
            }

            return Ok(category);
        }

        // PUT: api/Category/5
        [HttpPut("UpdateCategory/{id}")]
        public async Task<IActionResult> UpdateCategoryAsync(int id, [FromBody] Category category)
        {
            var cate = await _context.categories.FindAsync(id);
            if (cate == null)
            {
                return NotFound($"No Category was found with ID {id}");
            }
            cate.Name = category.Name;
            await _context.SaveChangesAsync();
            return Ok(cate);
        }

        // POST: api/Category
        [HttpPost("AddCategory")]
        public async Task<ActionResult<Category>> CreateCategoryAsync(Category cate)
        {
            var category = new Category { Name = cate.Name };
            await _context.AddAsync(category);
            _context.SaveChanges();

            return Ok(category);
        }

        // DELETE: api/Category/5
        [HttpDelete("DeleteCategoryByID/{id}")]
        public async Task<IActionResult> DeleteCategoryAsync(int id)
        {
            var category = await _context.categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            _context.Remove(category);
            await _context.SaveChangesAsync();

            return Ok(category);
        }

    }
}
