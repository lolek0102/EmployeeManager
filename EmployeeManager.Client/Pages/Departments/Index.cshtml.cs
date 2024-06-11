using EmployeeManager.Application.Dtos;
using EmployeeManager.Client.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace EmployeeManager.Client.Pages.Departments
{
    public class IndexModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly ApiService _apiService;

        public IndexModel(IConfiguration configuration, ApiService apiService)
        {
            _configuration = configuration;
            _apiService = apiService;
        }

        public List<DepartmentDto> Departments { get; set; } = new List<DepartmentDto>();
        public bool IsAdmin { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var isAuthenticated = JwtHelper.IsTokenValid(HttpContext, _configuration);
            if (!isAuthenticated)
            {
                return RedirectToPage("/Account/Login");
            }

            var principal = JwtHelper.GetClaimsPrincipal(HttpContext, _configuration);
            IsAdmin = principal?.Claims.Any(c => c.Type == ClaimTypes.Role && c.Value == "Admin") ?? false;

            Departments = await _apiService.GetAllDepartmentsAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var isAuthenticated = JwtHelper.IsTokenValid(HttpContext, _configuration);
            if (!isAuthenticated)
            {
                return RedirectToPage("/Account/Login");
            }

            var principal = JwtHelper.GetClaimsPrincipal(HttpContext, _configuration);
            var isAdmin = principal?.Claims.Any(c => c.Type == ClaimTypes.Role && c.Value == "Admin") ?? false;
            if (!isAdmin)
            {
                return RedirectToPage("/Account/AccessDenied");
            }

            var success = await _apiService.DeleteDepartmentAsync(id);
            if (!success)
            {
                return RedirectToPage("/Departments/DepartmentError");
            }

            return RedirectToPage();
        }
    }
}
