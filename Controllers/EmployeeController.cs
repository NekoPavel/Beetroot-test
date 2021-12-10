using Microsoft.AspNetCore.Mvc;
using API.Models;
using API.Services;

namespace API.Controllers;
[ApiController]
[Route("[controller]")]


public class EmployeeController : ControllerBase
{
    // GET: api/<EmployeeController>
    [HttpGet]
    public ActionResult<List<Employee>> GetAll() =>
        EmployeeService.GetAll();

    // GET api/<EmployeeController>/5
    [HttpGet("{id}")]
    public ActionResult<Employee> Get(int id)
    {
        var employee = EmployeeService.GetEmployeeByNumber(id);

        if (employee == null)
        {
            return NotFound();
        }
        return employee; 
    }

    // POST api/<EmployeeController>
    [HttpPost]
    public IActionResult Create(Employee employee)
    {
        EmployeeService.Add(employee);
        return CreatedAtAction(nameof(Create), new {id = employee.EmployeeNumber}, employee);
    }

    // PUT api/<EmployeeController>/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    // DELETE api/<EmployeeController>/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
        EmployeeService.Delete(id);
    }
}

