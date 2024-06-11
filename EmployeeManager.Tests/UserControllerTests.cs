using EmployeeManager.Application.Controllers;
using EmployeeManager.Application.Models;
using EmployeeManager.Application.Services;
using EmployeeManager.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EmployeeManager.Tests;

public class UserControllerTests
{
    private readonly Mock<IAuthService> _mockAuthService;
    private readonly UserController _controller;

    public UserControllerTests()
    {
        _mockAuthService = new Mock<IAuthService>();
        _controller = new UserController(_mockAuthService.Object);
    }

    [Fact]
    public async Task Login_ValidCredentials_ReturnsOkWithToken()
    {
        // Arrange
        var loginModel = new LoginRequest { Username = "validUser", Password = "validPass" };
        var user = new User { Username = "validUser", Role = "Admin" };
        _mockAuthService.Setup(s => s.ValidateUser(loginModel.Username, loginModel.Password))
                        .ReturnsAsync(user);
        _mockAuthService.Setup(s => s.GenerateJwtToken(user))
                        .Returns("GeneratedToken");

        // Act
        var result = await _controller.Login(loginModel);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var actual = Assert.IsType<LoginResponse>(okResult.Value);
        Assert.Equal("GeneratedToken", actual.Token);
        Assert.Equal("validUser", actual.Username);
        Assert.Equal("Admin", actual.Role);
    }

    [Fact]
    public async Task Login_InvalidCredentials_ReturnsUnauthorized()
    {
        // Arrange
        var loginModel = new LoginRequest { Username = "invalidUser", Password = "invalidPass" };
        _mockAuthService.Setup(s => s.ValidateUser(loginModel.Username, loginModel.Password))
                        .ReturnsAsync((User?)null);

        // Act
        var result = await _controller.Login(loginModel);

        // Assert
        var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
        Assert.Equal("Invalid username and/or password.", unauthorizedResult.Value);
    }
}
