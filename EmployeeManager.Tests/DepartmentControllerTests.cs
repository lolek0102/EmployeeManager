using EmployeeManager.Application.Controllers;
using EmployeeManager.Application.Dtos;
using EmployeeManager.Domain.Models;
using EmployeeManager.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EmployeeManager.Tests;

public class DepartmentControllerTests
{
    private readonly Mock<IDepartmentRepository> _mockRepo;
    private readonly DepartmentController _controller;

    public DepartmentControllerTests()
    {
        _mockRepo = new Mock<IDepartmentRepository>();
        _controller = new DepartmentController(_mockRepo.Object);
    }

    [Fact]
    public async Task GetAll_ReturnsAllDepartments()
    {
        // Arrange
        var departments = new List<Department>
        {
            new() { Id = 1, Name = "HR" },
            new() { Id = 2, Name = "IT" }
        };

        _mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(departments);

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedDepartments = Assert.IsType<List<Department>>(okResult.Value);
        Assert.Equal(2, returnedDepartments.Count);
    }

    [Fact]
    public async Task GetById_DepartmentExists_ReturnsCorrectDepartment()
    {
        // Arrange
        var departmentId = 1;
        var existingDepartment = new Department { Id = departmentId, Name = "HR" };

        _mockRepo.Setup(repo => repo.GetByIdAsync(departmentId))
                 .ReturnsAsync(existingDepartment);

        // Act
        var result = await _controller.GetById(departmentId);

        // Assert
        var okObjectResult = Assert.IsType<OkObjectResult>(result);
        var departmentDto = Assert.IsType<DepartmentDto>(okObjectResult.Value);

        // Optionally, check the content
        Assert.Equal(existingDepartment.Id, departmentDto.Id);
        Assert.Equal(existingDepartment.Name, departmentDto.Name);
    }


    [Fact]
    public async Task GetById_DepartmentDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        _mockRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((Department?)null);

        // Act
        var result = await _controller.GetById(1);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task Create_Department_ReturnsCreatedResult()
    {
        // Arrange
        var newDepartmentName = "Marketing";
        var newDepartment = new Department { Id = 3, Name = "Marketing" };

        _mockRepo.Setup(repo => repo.AddAsync(It.IsAny<string>()))
                 .ReturnsAsync(newDepartment);

        // Act
        var result = await _controller.Create(newDepartmentName);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal("Create", createdAtActionResult.ActionName);
        Assert.IsType<string>(createdAtActionResult.Value);

        var createdMessage = createdAtActionResult.Value as string;
        Assert.Equal($"Created department id: {newDepartment.Id}", createdMessage);
    }

    [Fact]
    public async Task Update_DepartmentExists_ReturnsNoContentResult()
    {
        // Arrange
        var departmentId = 1;
        var updatedName = "Updated Name";
        var department = new Department { Id = departmentId, Name = "Original Name" };

        _mockRepo.Setup(repo => repo.GetByIdAsync(departmentId)).ReturnsAsync(department);
        _mockRepo.Setup(repo => repo.UpdateAsync(departmentId, updatedName)).Verifiable();

        // Act
        var result = await _controller.Update(departmentId, updatedName);

        // Assert
        Assert.IsType<NoContentResult>(result);
        _mockRepo.Verify();
    }

    [Fact]
    public async Task Update_DepartmentDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        var departmentId = 1;
        var updatedName = "Updated Name";

        _mockRepo.Setup(repo => repo.GetByIdAsync(departmentId)).ReturnsAsync((Department?)null);

        // Act
        var result = await _controller.Update(departmentId, updatedName);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task Delete_DepartmentExists_ReturnsNoContent()
    {
        // Arrange
        var departmentId = 1;
        var existingDepartment = new Department { Id = departmentId, Name = "HR" };

        _mockRepo.Setup(repo => repo.GetByIdAsync(departmentId)).ReturnsAsync(existingDepartment);
        _mockRepo.Setup(repo => repo.DeleteAsync(departmentId)).Verifiable();

        // Act
        var result = await _controller.Delete(departmentId);

        // Assert
        Assert.IsType<NoContentResult>(result);
        _mockRepo.Verify(repo => repo.DeleteAsync(departmentId), Times.Once());
    }

    [Fact]
    public async Task Delete_DepartmentDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        var departmentId = 99;
        _mockRepo.Setup(repo => repo.GetByIdAsync(departmentId)).ReturnsAsync((Department?)null);

        // Act
        var result = await _controller.Delete(departmentId);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }


}
