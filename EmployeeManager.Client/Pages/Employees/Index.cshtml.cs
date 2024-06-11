using EmployeeManager.Application.Dtos;
using EmployeeManager.Client.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace EmployeeManager.Client.Pages.Employees
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

        public List<EmployeeDto> Employees { get; set; } = new List<EmployeeDto>();
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

            Employees = await _apiService.GetAllEmployeesAsync();

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

            await _apiService.DeleteEmployeeAsync(id);
            return RedirectToPage();
        }
    }
}
