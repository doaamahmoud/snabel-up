using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using snabel_up.DTO;
using snabel_up.Models;

namespace snabel_up.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private new List<string> allowExxtention = new List<string> { ".png", ".jpg" };

        public EmployeeController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet("GetAllEmployees")]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployeesAsync()
        {
            List<Employee> employees = await _context.employees.ToListAsync();
            return Ok(employees);
        }
        [HttpGet("GetEmployeeByID/{id}")]
        public async Task<ActionResult<Employee>> GetEmployeeByIDAsync(int id)
        {
            var employee = await _context.employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound($"No Employee was found with ID {id}");
            }

            return Ok(employee);
        }

        [HttpPost("AddEmployee")]
        public async Task<ActionResult<Employee>> CreateEmployeeAsync([FromForm] EmployeeDto emp)
        {
            if (emp.Image == null) return BadRequest("Image Is Required ");
            if (!allowExxtention.Contains(Path.GetExtension(emp.Image.FileName).ToLower()))
                return BadRequest("Onl .png or .jpg images are allow ");

            using var dataStream = new MemoryStream();
            await emp.Image.CopyToAsync(dataStream);
            var employee = new Employee
            {
                Name = emp.Name,
                Position=emp.Position,
                Image = dataStream.ToArray()
            };
            await _context.AddAsync(employee);
            _context.SaveChanges();

            return Ok(employee);

        }


        [HttpPut("UpdateEmployeeByID/{id}")]
        public async Task<IActionResult> UpdateEmployeeAsync(int id, [FromForm] EmployeeDto emp)
        {
            var employee = await _context.employees.FindAsync(id);
            if (employee == null)
                return NotFound($"No Employee was found with ID {id}");
            if (emp.Image != null)
            {
                if (!allowExxtention.Contains(Path.GetExtension(emp.Image.FileName).ToLower()))
                    return BadRequest("Onl .png or .jpg images are allow ");
                using var dataStream = new MemoryStream();
                await emp.Image.CopyToAsync(dataStream);
                employee.Image = dataStream.ToArray();
            }
            employee.Name = emp.Name;
            employee.Position= emp.Position;
            await _context.SaveChangesAsync();
            return Ok(employee);
        }



        [HttpDelete("DeleteEmployeeByID/{id}")]
        public async Task<IActionResult> DeleteEmployeeAsync(int id)
        {
            var employee = await _context.employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            _context.Remove(employee);
            await _context.SaveChangesAsync();

            return Ok(employee);
        }
    }
}
