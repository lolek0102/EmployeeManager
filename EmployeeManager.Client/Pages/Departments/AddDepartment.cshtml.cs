using EmployeeManager.Client.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace EmployeeManager.Client.Pages.Departments
{
    public class AddDepartmentModel : PageModel
    {
        private readonly ApiService _apiService;
        private readonly IConfiguration _configuration;

        public AddDepartmentModel(ApiService apiService, IConfiguration configuration)
        {
            _apiService = apiService;
            _configuration = configuration;
        }

        [BindProperty]
        public string DepartmentName { get; set; }

        public async Task<IActionResult> OnGetAsync()
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

            await _apiService.CreateDepartmentAsync(DepartmentName);
            return RedirectToPage("/Departments/Index");
        }
    }
}
