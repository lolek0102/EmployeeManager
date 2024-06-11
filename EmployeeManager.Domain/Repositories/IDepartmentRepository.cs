using EmployeeManager.Domain.Models;

namespace EmployeeManager.Domain.Repositories;

public interface IDepartmentRepository
{
    Task<List<Department>> GetAllAsync();
    Task<Department?> GetByIdAsync(int id);
    Task<Department> AddAsync(string name);
    Task UpdateAsync(int id, string department);
    Task DeleteAsync(int id);
}