using EmployeeManager.Application.Controllers;
using EmployeeManager.Application.Dtos;
using EmployeeManager.Domain.Models;
using EmployeeManager.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EmployeeManager.Tests;

public class EmployeeControllerTests
{
    private readonly Mock<IEmployeeRepository> _mockEmployeeRepo;
    private readonly Mock<IDepartmentRepository> _mockDepartmentRepo;
    private readonly EmployeeController _controller;

    public EmployeeControllerTests()
    {
        _mockEmployeeRepo = new Mock<IEmployeeRepository>();
        _mockDepartmentRepo = new Mock<IDepartmentRepository>();
        _controller = new EmployeeController(_mockDepartmentRepo.Object, _mockEmployeeRepo.Object);
    }

    [Fact]
    public async Task GetAll_ReturnsAllEmployees()
    {
        // Arrange
        var employees = new List<Employee>
    {
        new() {
            Id = 1,
            FirstName = "John",
            LastName = "Doe",
            DepartmentId = 1,
            Department = new Department { Id = 1, Name = "HR" },
            Address = new Address { Street = "123 Main St", City = "Anytown", State = "Anystate" }
        },
        new() {
            Id = 2,
            FirstName = "Jane",
            LastName = "Smith",
            DepartmentId = 2,
            Department = new Department { Id = 2, Name = "IT" },
            Address = new Address { Street = "456 Elm St", City = "Othertown", State = "Otherstate" }
        }
    };

        _mockEmployeeRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(employees);

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedEmployees = Assert.IsType<List<EmployeeDto>>(okResult.Value);
        Assert.Equal(2, returnedEmployees.Count);
    }


    [Fact]
    public async Task GetById_EmployeeExists_ReturnsCorrectEmployee()
    {
        // Arrange
        var employeeId = 1;
        var existingEmployee = new Employee
        {
            Id = employeeId,
            FirstName = "John",
            LastName = "Doe",
            DepartmentId = 1,
            Department = new Department { Id = 1, Name = "IT" },
            Address = new Address { Street = "123 Main St", City = "Anytown", State = "Anystate" }
        };

        _mockEmployeeRepo.Setup(repo => repo.GetByIdAsync(employeeId))
                         .ReturnsAsync(existingEmployee);

        // Act
        var result = await _controller.GetByIdAsync(employeeId);

        // Assert
        var okObjectResult = Assert.IsType<OkObjectResult>(result);
        var employeeDto = Assert.IsType<EmployeeDto>(okObjectResult.Value);

        // Optionally, check the content
        Assert.Equal(existingEmployee.Id, employeeDto.Id);
        Assert.Equal(existingEmployee.FirstName, employeeDto.FirstName);
        Assert.Equal(existingEmployee.LastName, employeeDto.LastName);
    }

    [Fact]
    public async Task GetById_EmployeeDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        _mockEmployeeRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((Employee?)null);

        // Act
        var result = await _controller.GetByIdAsync(1);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task Create_Employee_ReturnsCreatedResult()
    {
        // Arrange
        var newEmployeeDto = new CreateEmployeeDto
        {
            FirstName = "New",
            LastName = "Employee",
            DepartmentId = 1,
            Street = "123 Street",
            City = "City",
            State = "State"
        };
        var newEmployee = new Employee
        {
            Id = 3,
            FirstName = "New",
            LastName = "Employee",
            DepartmentId = 1
        };

        _mockDepartmentRepo.Setup(repo => repo.GetByIdAsync(newEmployeeDto.DepartmentId))
                           .ReturnsAsync(new Department());
        _mockEmployeeRepo.Setup(repo => repo.AddAsync(It.IsAny<Employee>()))
                         .ReturnsAsync(newEmployee);

        // Act
        var result = await _controller.Create(newEmployeeDto);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal("Create", createdAtActionResult.ActionName);
        Assert.Equal($"Created employee id: {newEmployee.Id}", createdAtActionResult.Value as string);
    }

    [Fact]
    public async Task Update_EmployeeExists_ReturnsNoContentResult()
    {
        // Arrange
        var employeeId = 1;
        var newEmployeeDto = new EmployeeDto
        {
            Id = employeeId,
            FirstName = "Updated",
            LastName = "Name",
            DepartmentId = 1,
            Street = "123 New St",
            City = "New City",
            State = "New State"
        };
        var existingEmployee = new Employee
        {
            Id = employeeId,
            FirstName = "Original",
            LastName = "Name",
            DepartmentId = 1,
            Address = new Address { Street = "123 Main St", City = "Anytown", State = "Anystate" }
        };

        _mockEmployeeRepo.Setup(repo => repo.GetByIdAsync(employeeId)).ReturnsAsync(existingEmployee);
        _mockDepartmentRepo.Setup(repo => repo.GetByIdAsync(newEmployeeDto.DepartmentId)).ReturnsAsync(new Department());
        _mockEmployeeRepo.Setup(repo => repo.UpdateAsync(It.IsAny<Employee>())).Verifiable();

        // Act
        var result = await _controller.Update(newEmployeeDto);

        // Assert
        Assert.IsType<NoContentResult>(result);
        _mockEmployeeRepo.Verify();
    }

    [Fact]
    public async Task Update_EmployeeDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        var employeeDto = new EmployeeDto
        {
            Id = 1,
            FirstName = "Updated",
            LastName = "Name",
            DepartmentId = 1,
            Street = "123 New St",
            City = "New City",
            State = "New State"
        };

        _mockEmployeeRepo.Setup(repo => repo.GetByIdAsync(employeeDto.Id)).ReturnsAsync((Employee?)null);

        // Act
        var result = await _controller.Update(employeeDto);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task Delete_EmployeeExists_ReturnsNoContentResult()
    {
        // Arrange
        var employeeId = 1;
        var existingEmployee = new Employee { Id = employeeId };

        _mockEmployeeRepo.Setup(repo => repo.GetByIdAsync(employeeId)).ReturnsAsync(existingEmployee);
        _mockEmployeeRepo.Setup(repo => repo.DeleteAsync(employeeId)).Verifiable();

        // Act
        var result = await _controller.Delete(employeeId);

        // Assert
        Assert.IsType<NoContentResult>(result);
        _mockEmployeeRepo.Verify();
    }

    [Fact]
    public async Task Delete_EmployeeDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        _mockEmployeeRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((Employee?)null);

        // Act
        var result = await _controller.Delete(1);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }
}
