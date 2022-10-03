using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using snabel_up.Models;
namespace snabel_up.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BranchController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BranchController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Branches
        [HttpGet("GetAllBranches")]
        public async Task<ActionResult<IEnumerable<Branch>>> GetBranchesAsync()
        {
            List<Branch> branches = await _context.branches.ToListAsync();
            return Ok(branches);
        }

        // GET: api/Branch/5
        [HttpGet("GetBranchByID/{id}")]
        public async Task<ActionResult<Branch>> GetBranchAsync(int id)
        {
            var branch = await _context.branches.FindAsync(id);

            if (branch == null)
            {
                return NotFound($"No branch was found with ID {id}");
            }

            return Ok(branch);
        }

        // PUT: api/Branch/5
      
        [HttpPut("UpdateBranch/{id}")]
        public async Task<IActionResult> UpdateBranchAsync(int id, [FromBody] Branch brn)
        {
            var branch = await _context.branches.FindAsync(id);
            if (branch == null)
            {
                return NotFound($"No Category was found with ID {id}");
            }
             branch.Name= brn.Name;
             branch.Address= brn.Address;
             branch.Email= brn.Email;
             branch.Phone1= brn.Phone1;
             branch.Phone2= brn.Phone2;
            await _context.SaveChangesAsync();
            return Ok(branch);
        }

        // POST: api/Branch
        [HttpPost("AddBranch")]
        public async Task<ActionResult<Branch>> CreateBranchAsync(Branch brn)
        {
            var branch = new Branch { Name = brn.Name };
            await _context.AddAsync(branch);
            _context.SaveChanges();
            return Ok(branch);
        }

        // DELETE: api/Branch/5
        [HttpDelete("DeleteBranchByID/{id}")]
        public async Task<IActionResult> DeleteBranchAsync(int id)
        {
            var branch = await _context.branches.FindAsync(id);
            if (branch == null)
            {
                return NotFound();
            }

            _context.Remove(branch);
            await _context.SaveChangesAsync();

            return Ok(branch);
        }
    }
}
