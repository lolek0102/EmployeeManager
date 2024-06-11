using EmployeeManager.Application.Dtos;
using EmployeeManager.Application.Models;
using System.Net.Http.Headers;

public class ApiService
{
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ApiService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClient;
        _httpContextAccessor = httpContextAccessor;
    }

    private void AddJwtToken()
    {
        var token = _httpContextAccessor.HttpContext?.Request.Cookies["Token"];
        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }

    public async Task<LoginResponse?> LoginAsync(LoginRequest loginRequest)
    {
        var response = await _httpClient.PostAsJsonAsync("api/user/login", loginRequest);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<LoginResponse>();
        }

        return null;
    }

    public async Task<List<EmployeeDto>> GetAllEmployeesAsync()
    {
        AddJwtToken();
        var response = await _httpClient.GetAsync("api/employee/all");
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<List<EmployeeDto>>() ?? new List<EmployeeDto>();
        }
        return new List<EmployeeDto>();
    }

    public async Task<EmployeeDto?> GetEmployeeByIdAsync(int id)
    {
        AddJwtToken();
        var response = await _httpClient.GetAsync($"api/employee/{id}");
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<EmployeeDto>();
        }
        return null;
    }

    public async Task UpdateEmployeeAsync(EmployeeDto employee)
    {
        AddJwtToken();
        await _httpClient.PutAsJsonAsync("api/employee/update", employee);
    }

    public async Task CreateEmployeeAsync(CreateEmployeeDto employee)
    {
        AddJwtToken();
        await _httpClient.PostAsJsonAsync("api/employee", employee);
    }

    public async Task DeleteEmployeeAsync(int id)
    {
        AddJwtToken();
        await _httpClient.DeleteAsync($"api/employee/{id}");
    }

    public async Task<List<DepartmentDto>> GetAllDepartmentsAsync()
    {
        AddJwtToken();
        var response = await _httpClient.GetAsync("api/department/all");
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<List<DepartmentDto>>() ?? new List<DepartmentDto>();
        }
        return new List<DepartmentDto>();
    }

    public async Task<DepartmentDto?> GetDepartmentByIdAsync(int id)
    {
        AddJwtToken();
        var response = await _httpClient.GetAsync($"api/department/{id}");
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<DepartmentDto>();
        }
        return null;
    }

    public async Task UpdateDepartmentAsync(DepartmentDto department)
    {
        AddJwtToken();
        await _httpClient.PutAsJsonAsync($"api/department/{department.Id}", department.Name);
    }

    public async Task<bool> DeleteDepartmentAsync(int id)
    {
        AddJwtToken();
        var response = await _httpClient.DeleteAsync($"api/department/{id}");
        return response.IsSuccessStatusCode;
    }

    public async Task CreateDepartmentAsync(string departmentName)
    {
        AddJwtToken();
        await _httpClient.PostAsJsonAsync("api/department", departmentName);
    }

}
