using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using snabel_up.Models;

namespace snabel_up.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupCategoryController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SupCategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        
        [HttpGet("GetAllSupCategories")]
        public async Task<ActionResult<IEnumerable<SupCategory>>> GetSupCategoriesAsync()
        {
            List<SupCategory> SupCategories = await _context.supCategories.Include(p => p.Category).ToListAsync();
            return Ok(SupCategories);
        }

    
        [HttpGet("GetSupCategoryById/{id}")]
        public async Task<ActionResult<SupCategory>> GetSupCategoryAsync(int id)
        {
            var Supcategory = await _context.supCategories.FindAsync(id);

            if (Supcategory == null)
            {
                return NotFound($"No category was found with ID {id}");
            }

            return Ok(Supcategory);
        }


        [HttpPut("UpdateSupCategory/{id}")]
        public async Task<IActionResult> UpdateSupCategoryAsync(int id, [FromBody] SupCategory supCategory)
        {
            var Supcate = await _context.supCategories.FindAsync(id);
            if (Supcate == null)
            {
                return NotFound($"No  SupCategory was found with ID {id}");
            }
            Supcate.Name = supCategory.Name;
            await _context.SaveChangesAsync();
            return Ok(Supcate);
        }


        [HttpPost("AddSupCategory")]
        public async Task<ActionResult<SupCategory>> CreateSupCategoryAsync(SupCategory supcate)
        {
            var Supcategory = new SupCategory { Name = supcate.Name };
            await _context.AddAsync(Supcategory);
            _context.SaveChanges();

            return Ok(Supcategory);
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
