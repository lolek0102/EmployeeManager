using EmployeeManager.Application.Models;
using EmployeeManager.Application.Services;
using EmployeeManager.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManager.Application.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController(IAuthService _authService) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest login)
    {
        User? user = await _authService.ValidateUser(login.Username, login.Password);
        if (user is null)
        {
            return Unauthorized("Invalid username and/or password.");
        }

        string token = _authService.GenerateJwtToken(user);
        LoginResponse response = new()
        {
            Token = token,
            Username = user.Username,
            Role = user.Role
        };
        return Ok(response);
    }
}
