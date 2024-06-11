using EmployeeManager.Application.Dtos;
using EmployeeManager.Domain.Models;
using EmployeeManager.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManager.Application.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DepartmentController(IDepartmentRepository _departmentRepository) : ControllerBase
{
    [Authorize]
    [HttpGet("all")]
    public async Task<IActionResult> GetAll()
    {
        List<Department> departments = await _departmentRepository.GetAllAsync();
        return Ok(departments);
    }
    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        Department? department = await _departmentRepository.GetByIdAsync(id);
        if (department is null)
        {
            return NotFound("Department not found.");
        }

        var departmentDto = new DepartmentDto
        {
            Id = department.Id,
            Name = department.Name,
            Employees = department.Employees.Select(e => new EmployeeDto
            {
                Id = e.Id,
                FirstName = e.FirstName,
                LastName = e.LastName,
                DepartmentId = e.DepartmentId,
                Street = e.Address.Street,
                City = e.Address.City,
                State = e.Address.State
            }).ToList()
        };

        return Ok(departmentDto);
    }
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] string name)
    {
        Department newDepartment = await _departmentRepository.AddAsync(name);
        return CreatedAtAction("Create", $"Created department id: {newDepartment.Id}");
    }
    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] string name)
    {
        if (await _departmentRepository.GetByIdAsync(id) is null)
        {
            return NotFound("Department not found.");
        }

        await _departmentRepository.UpdateAsync(id, name);
        return NoContent();
    }
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        Department? departmentToDelete = await _departmentRepository.GetByIdAsync(id);
        if (departmentToDelete is null)
        {
            return NotFound("Department not found.");
        }

        if (departmentToDelete.Employees.Count != 0)
        {
            return BadRequest("Department containing employees cannot be deleted.");
        }

        await _departmentRepository.DeleteAsync(id);
        return NoContent();
    }
}
