using EmployeeManager.Domain.Models;
using EmployeeManager.Domain.Repositories;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace EmployeeManager.Application.Services;

public interface IAuthService
{
    string GenerateHash(string password);
    string GenerateJwtToken(User user);
    Task<User?> ValidateUser(string username, string password);
}

public class AuthService(IUserRepository _userRepository, IConfiguration _configuration) : IAuthService
{
    public async Task<User?> ValidateUser(string username, string password)
    {
        User? user = await _userRepository.GetByUsernameAsync(username);
        string hash = GenerateHash(password);
        if (user is not null && hash == user.PasswordHash)
        {
            return user;
        }
        return null;
    }
    public string GenerateJwtToken(User user)
    {
        SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]!));
        SigningCredentials credentials = new(key, SecurityAlgorithms.HmacSha256);
        DateTime expireTime = DateTime.Now.AddHours(1);

        Claim[] claims = [
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role)];

        JwtSecurityToken token = new("localhost",
            "localhost",
            expires: expireTime,
            signingCredentials: credentials,
            claims: claims);

        JwtSecurityTokenHandler tokenHandler = new();
        return tokenHandler.WriteToken(token);
    }

    public string GenerateHash(string password)
    {
        byte[] hashedBytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));

        StringBuilder builder = new();
        for (int i = 0; i < hashedBytes.Length; i++)
        {
            builder.Append(hashedBytes[i].ToString("x2"));
        }

        return builder.ToString();
    }
}
