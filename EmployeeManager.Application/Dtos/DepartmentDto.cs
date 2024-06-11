namespace EmployeeManager.Application.Dtos;

public class DepartmentDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<EmployeeDto> Employees { get; set; } = [];
}
