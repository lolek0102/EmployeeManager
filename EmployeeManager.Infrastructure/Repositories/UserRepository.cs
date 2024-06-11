using EmployeeManager.Domain.Models;
using EmployeeManager.Domain.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManager.Infrastructure.Repositories;

public class UserRepository(AppDbContext _context) : IUserRepository
{
    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _context.Users.FirstOrDefaultAsync(x => x.Username == username);
    }
}
