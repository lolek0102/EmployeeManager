using EmployeeManager.Application.Dtos;
using EmployeeManager.Domain.Models;
using EmployeeManager.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManager.Application.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EmployeeController(IDepartmentRepository _departmentRepository, IEmployeeRepository _employeeRepository) : ControllerBase
{
    [Authorize]
    [HttpGet("all")]
    public async Task<IActionResult> GetAll()
    {
        List<Employee> employees = await _employeeRepository.GetAllAsync();
        List<EmployeeDto> dtos = employees.Select(e => new EmployeeDto
        {
            Id = e.Id,
            FirstName = e.FirstName,
            LastName = e.LastName,
            DepartmentId = e.Department.Id,
            DepartmentName = e.Department.Name,
            Street = e.Address.Street,
            City = e.Address.City,
            State = e.Address.State
        }).ToList();
        return Ok(dtos);
    }
    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(int id)
    {
        Employee? employee = await _employeeRepository.GetByIdAsync(id);
        if (employee is null)
        {
            return NotFound("Employee not found.");
        }
        EmployeeDto dto = new()
        {
            Id = employee.Id,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            DepartmentId = employee.Department.Id,
            DepartmentName = employee.Department.Name,
            Street = employee.Address.Street,
            City = employee.Address.City,
            State = employee.Address.State
        };
        return Ok(dto);
    }
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateEmployeeDto employeeDto)
    {
        Department? department = await _departmentRepository.GetByIdAsync(employeeDto.DepartmentId);
        if (department is null)
        {
            return NotFound("Department not found.");
        }
        Employee employee = new()
        {
            FirstName = employeeDto.FirstName,
            LastName = employeeDto.LastName,
            DepartmentId = employeeDto.DepartmentId,
            Address = new()
            {
                Street = employeeDto.Street,
                City = employeeDto.City,
                State = employeeDto.State
            }
        };
        Employee newEmployee = await _employeeRepository.AddAsync(employee);
        return CreatedAtAction("Create", $"Created employee id: {newEmployee.Id}");
    }
    [Authorize(Roles = "Admin")]
    [HttpPut("update")]
    public async Task<IActionResult> Update([FromBody] EmployeeDto newEmployee)
    {
        Employee? existingEmployee = await _employeeRepository.GetByIdAsync(newEmployee.Id);
        if (existingEmployee is null)
        {
            return NotFound("Employee not found.");
        }

        Department? department = await _departmentRepository.GetByIdAsync(newEmployee.DepartmentId);
        if (department is null)
        {
            return NotFound("Department not found.");
        }

        existingEmployee.FirstName = newEmployee.FirstName;
        existingEmployee.LastName = newEmployee.LastName;
        existingEmployee.DepartmentId = newEmployee.DepartmentId;
        existingEmployee.Address.Street = newEmployee.Street;
        existingEmployee.Address.City = newEmployee.City;
        existingEmployee.Address.State = newEmployee.State;

        await _employeeRepository.UpdateAsync(existingEmployee);
        return NoContent();
    }
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        Employee? employee = await _employeeRepository.GetByIdAsync(id);

        if (employee is null)
        {
            return NotFound("Employee not found.");
        }

        await _employeeRepository.DeleteAsync(id);
        return NoContent();
    }
}
