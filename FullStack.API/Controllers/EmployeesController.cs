using FullStack.API.Data;
using FullStack.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FullStack.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class EmployeesController : Controller
    {
        private readonly FullStackDbContext _fullStackDbContext;

        public EmployeesController(FullStackDbContext fullStackDbContext)
        {
            _fullStackDbContext = fullStackDbContext;
        }
        // Get All Employees
        [HttpGet]
        public async Task<IActionResult> GetAllEmployees()
        {
            var employees = await _fullStackDbContext.Employees.ToListAsync();
            return Ok(employees);
        }
        // Get Single a Employee
        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetEmployees")]
        public async Task<ActionResult> GetEmployees([FromRoute] Guid id)
        {
            var employees = await _fullStackDbContext.Employees.FirstOrDefaultAsync(x => x.Id == id);
            if (employees != null)
            {
                return Ok(employees);
            }
            return NotFound("Employee Not found");
        }
        // Add a Employee
        [HttpPost]
        public async Task<IActionResult> AddEmployee([FromBody] Employee employees)
        {
            employees.Id = Guid.NewGuid();
            await _fullStackDbContext.Employees.AddAsync(employees);
            await _fullStackDbContext.SaveChangesAsync();
          return CreatedAtAction(nameof(GetEmployees), new { id = employees.Id }, employees);
        }
        // Update a Employee
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateEmployees([FromRoute] Guid id, [FromBody] Employee employees)
        {
           // var existingEmployee = await _fullStackDbContext.Employees.FirstOrDefaultAsync(x => x.Id == id);
            var existingEmployee = await _fullStackDbContext.Employees.FindAsync(id);
            if (existingEmployee != null)
            {
                existingEmployee.Name = employees.Name;
                existingEmployee.Phone = employees.Phone;
                existingEmployee.Email = employees.Email;
                existingEmployee.Salary = employees.Salary;
                existingEmployee.Department = employees.Department;
                await _fullStackDbContext.SaveChangesAsync();
                return Ok(existingEmployee);
            }
            return NotFound("Employee not found");
        }
        // Delete a Employee
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteEmployee([FromRoute] Guid id)
        {
            var employee = await _fullStackDbContext.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            _fullStackDbContext.Employees.Remove(employee);
            await _fullStackDbContext.SaveChangesAsync();
            return Ok(employee);
        }
    }
}
