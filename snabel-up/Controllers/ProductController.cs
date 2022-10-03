using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using snabel_up.Models;
using snabel_up.DTO;

namespace snabel_up.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private new List<string> allowExxtention = new List<string> { ".png", ".jpg" };

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet("GetAllProducts")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductesAsync()
        {
            List<Product> products = await _context.products.Include(p => p.Category).ToListAsync();
            return Ok(products);
        }
        [HttpGet("GetProductByID/{id}")]
        public async Task<ActionResult<Product>> GetProductByIDAsync(int id)
        {
            var product = await _context.products.FindAsync(id);

            if (product == null)
            {
                return NotFound($"No Product was found with ID {id}");
            }

            return Ok(product);
        }
        [HttpGet("GetProductByCategorID")]
        public async Task<ActionResult<Product>> GetProductByCategorIDAsync(int CatID)
        {
            var products = await _context.products.Where(p => p.Category_Id == CatID).ToListAsync();

            return Ok(products);
        }
        [HttpPost("AddProduct")]
        public async Task<ActionResult<Product>> CreateProductAsync([FromForm] ProductDto pro)
        {
            if (pro.Image == null) return BadRequest("Image Is Required ");
            if (!allowExxtention.Contains(Path.GetExtension(pro.Image.FileName).ToLower()))
                return BadRequest("Onl .png or .jpg images are allow ");
            var isValidCategory = await _context.categories.AnyAsync(c => c.Id == pro.Category_Id);
            if (!isValidCategory)
                return BadRequest("Not Valide Category Id ");
            using var dataStream = new MemoryStream();
            await pro.Image.CopyToAsync(dataStream);
            var product = new Product
            {

                Name = pro.Name,
                price = pro.price,
                Quantity = pro.Quantity,
                Image = dataStream.ToArray(),
                Description = pro.Description,
                Category_Id = pro.Category_Id


            };
            await _context.AddAsync(product);
            _context.SaveChanges();

            return Ok(product);

        }
        [HttpPut("UpdateProduct/{id}")]
        public async Task<IActionResult> UpdateProductAsync(int id, [FromForm] ProductDto product)
        {
            var pro = await _context.products.FindAsync(id);
            if (pro == null)
                return NotFound($"No Product was found with ID {id}");
            var isValidCategory = await _context.categories.AnyAsync(c => c.Id == pro.Category_Id);
            if (!isValidCategory)
                return BadRequest("Not Valide Category Id ");
            if (product.Image != null)
            {
                if (!allowExxtention.Contains(Path.GetExtension(product.Image.FileName).ToLower()))
                    return BadRequest("Onl .png or .jpg images are allow ");
                using var dataStream = new MemoryStream();
                await product.Image.CopyToAsync(dataStream);
                pro.Image = dataStream.ToArray();
            }
            pro.Name = product.Name;
            pro.price = product.price;
            pro.Quantity = product.Quantity;
            pro.Description = product.Description;
            pro.Category_Id = product.Category_Id;
            await _context.SaveChangesAsync();
            return Ok(pro);
        }



        [HttpDelete("DeleteProductByID/{id}")]
        public async Task<IActionResult> DeleteProductAsync(int id)
        {
            var product = await _context.products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Remove(product);
            await _context.SaveChangesAsync();

            return Ok(product);
        }
    }
}
