using EmployeeManager.Domain.Models;
using Infrastructure.Data;
using System.Security.Cryptography;
using System.Text;

namespace EmployeeManager.Infrastructure;
public class DataSeeder(AppDbContext _context)
{
    public void Seed()
    {
        SeedUsers();
        SeedDepartments();
        SeedEmployees();
    }

    private void SeedUsers()
    {
        User admin = new() { Username = "admin", PasswordHash = GenerateHash("admin"), Role = "Admin" };
        User user = new() { Username = "user", PasswordHash = GenerateHash("user"), Role = "User" };
        _context.Users.AddRange(admin, user);
        _context.SaveChanges();
    }

    private void SeedDepartments()
    {
        Department sales = new() { Name = "Sales" };
        Department hr = new() { Name = "Human Resources" };
        _context.Departments.AddRange(sales, hr);
        _context.SaveChanges();
    }

    private void SeedEmployees()
    {
        Employee employee1 = new()
        {
            FirstName = "John",
            LastName = "Smith",
            DepartmentId = 1,
            Address = new Address { Street = "155B Baker Street", City = "London", State = "Great Britain" }
        };
        Employee employee2 = new()
        {
            FirstName = "Jane",
            LastName = "Doe",
            DepartmentId = 2,
            Address = new Address { Street = "17 Wall Street", City = "New York", State = "USA" }
        };
        Employee employee3 = new()
        {
            FirstName = "Hans",
            LastName = "Schmidt",
            DepartmentId = 2,
            Address = new Address { Street = "14 Industriestrasse", City = "Berlin", State = "Germany" }
        };
        _context.Employees.AddRange(employee1, employee2, employee3);
        _context.SaveChanges();
    }

    private static string GenerateHash(string input)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(input);
        byte[] hash = SHA256.HashData(bytes);
        return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
    }
}
