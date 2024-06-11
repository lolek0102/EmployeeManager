using EmployeeManager.Application.Dtos;
using EmployeeManager.Client.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace EmployeeManager.Client.Pages.Departments
{
    public class EditDepartmentModel : PageModel
    {
        private readonly ApiService _apiService;
        private readonly IConfiguration _configuration;

        public EditDepartmentModel(ApiService apiService, IConfiguration configuration)
        {
            _apiService = apiService;
            _configuration = configuration;
        }

        [BindProperty]
        public DepartmentDto Department { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var isAuthenticated = JwtHelper.IsTokenValid(HttpContext, _configuration);
            if (!isAuthenticated)
            {
                return RedirectToPage("/Account/Login");
            }

            Department = await _apiService.GetDepartmentByIdAsync(id);

            if (Department == null)
            {
                return NotFound();
            }

            var principal = JwtHelper.GetClaimsPrincipal(HttpContext, _configuration);
            var isAdmin = principal?.Claims.Any(c => c.Type == ClaimTypes.Role && c.Value == "Admin") ?? false;
            if (!isAdmin)
            {
                return RedirectToPage("/Account/AccessDenied");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
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

            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _apiService.UpdateDepartmentAsync(Department);
            return RedirectToPage("/Departments/Index");
        }
    }
}
