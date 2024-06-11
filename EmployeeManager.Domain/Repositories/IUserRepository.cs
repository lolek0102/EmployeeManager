using EmployeeManager.Domain.Models;

namespace EmployeeManager.Domain.Repositories;

public interface IUserRepository
{
    public Task<User?> GetByUsernameAsync(string username);
}
