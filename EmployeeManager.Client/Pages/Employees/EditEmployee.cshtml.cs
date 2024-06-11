using EmployeeManager.Application.Dtos;
using EmployeeManager.Client.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace EmployeeManager.Client.Pages.Employees
{
    public class EditEmployeeModel : PageModel
    {
        private readonly ApiService _apiService;
        private readonly IConfiguration _configuration;

        public EditEmployeeModel(ApiService apiService, IConfiguration configuration)
        {
            _apiService = apiService;
            _configuration = configuration;
        }

        [BindProperty]
        public EmployeeDto Employee { get; set; }

        public List<SelectListItem> DepartmentList { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var isAuthenticated = JwtHelper.IsTokenValid(HttpContext, _configuration);
            if (!isAuthenticated)
            {
                return RedirectToPage("/Account/Login");
            }

            Employee = await _apiService.GetEmployeeByIdAsync(id);

            if (Employee == null)
            {
                return NotFound();
            }

            var principal = JwtHelper.GetClaimsPrincipal(HttpContext, _configuration);
            var isAdmin = principal?.Claims.Any(c => c.Type == ClaimTypes.Role && c.Value == "Admin") ?? false;
            if (!isAdmin)
            {
                return RedirectToPage("/Account/AccessDenied");
            }

            var departments = await _apiService.GetAllDepartmentsAsync();
            DepartmentList = departments.Select(d => new SelectListItem { Value = d.Id.ToString(), Text = d.Name }).ToList();

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
                var departments = await _apiService.GetAllDepartmentsAsync();
                DepartmentList = departments.Select(d => new SelectListItem { Value = d.Id.ToString(), Text = d.Name }).ToList();
                return Page();
            }

            await _apiService.UpdateEmployeeAsync(Employee);
            return RedirectToPage("/Employees/Index");
        }
    }
}
