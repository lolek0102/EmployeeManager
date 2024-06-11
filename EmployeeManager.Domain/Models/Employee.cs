namespace EmployeeManager.Domain.Models;

public class Employee
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int DepartmentId { get; set; }
    public Department Department { get; set; } = default!;
    public int AddressId { get; set; } = default!;
    public Address Address { get; set; } = default!;
}
