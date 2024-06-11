using EmployeeManager.Domain.Models;
using EmployeeManager.Domain.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManager.Infrastructure.Repositories;

public class DepartmentRepository(AppDbContext _context) : IDepartmentRepository
{
    public async Task<List<Department>> GetAllAsync()
    {
        return await _context.Departments.ToListAsync();
    }

    public async Task<Department?> GetByIdAsync(int id)
    {
        Department? department = await _context.Departments.Include(d => d.Employees).ThenInclude(e => e.Address).FirstOrDefaultAsync(d => d.Id == id);
        return department;
    }

    public async Task<Department> AddAsync(string name)
    {
        Department department = new() { Name = name }; _context.Departments.Add(department);
        await _context.SaveChangesAsync();
        return department;
    }

    public async Task UpdateAsync(int id, string name)
    {
        Department? department = await _context.Departments.FindAsync(id);
        if (department is null)
        {
            return;
        }
        department.Name = name;
        _context.Departments.Update(department);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        Department? department = await _context.Departments.FindAsync(id);
        if (department is not null)
        {
            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();
        }
    }
}
