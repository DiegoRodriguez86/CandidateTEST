using CandidateTEST.Models.DB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace CandidateTEST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly AppDBContext _appDBContext;

        public EmployeeController(AppDBContext context)
        {
            _appDBContext = context;
        }

        [HttpPost("CreateEmployee")]
        public async Task<IActionResult> CreateEmployee(Employee employee) 
        {
            if (employee == null) return BadRequest("Invalid request");

            bool validateRfcExist = await _appDBContext.Employees.AnyAsync(p => p.RFC == employee.RFC);
            if (validateRfcExist) return Conflict("Employee with the same RFC already exists");

            //regular expression obtained from https://gist.github.com/edgaryahir-angeles/4492a51d784646b71104f59610bec043
            bool validateFormatRFC = Regex.IsMatch(employee.RFC, "^(([A-ZÑ&]{4})([0-9]{2})([0][13578]|[1][02])(([0][1-9]|[12][\\d])|[3][01])([A-Z0-9]{3}))|(([A-ZÑ&]{4})([0-9]{2})([0][13456789]|[1][012])(([0][1-9]|[12][\\d])|[3][0])([A-Z0-9]{3}))|(([A-ZÑ&]{4})([02468][048]|[13579][26])[0][2]([0][1-9]|[12][\\d])([A-Z0-9]{3}))|(([A-ZÑ&]{4})([0-9]{2})[0][2]([0][1-9]|[1][0-9]|[2][0-8])([A-Z0-9]{3}))$");
            if (!validateFormatRFC) return BadRequest("Invalid RFC format or length");

            await _appDBContext.Employees.AddAsync(employee);
            await _appDBContext.SaveChangesAsync();
            return Ok("Employee created successfully");
        }

        [HttpGet("GetEmpleados")]
        public async Task<IActionResult> GetEmpleados(string? name)
        {
            var result = new List<Employee>();
            result = await _appDBContext.Employees.OrderBy(p => p.BornDate).ToListAsync();
            if(!string.IsNullOrEmpty(name))
            {
                result = result.Where(p => p.Name.Contains(name)).ToList();
            }
            return Ok(result);
        }
    }
}
