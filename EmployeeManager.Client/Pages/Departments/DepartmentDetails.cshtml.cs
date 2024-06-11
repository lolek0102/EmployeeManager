using EmployeeManager.Application.Dtos;
using EmployeeManager.Client.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployeeManager.Client.Pages.Departments
{
    public class DepartmentDetailsModel : PageModel
    {
        private readonly ApiService _apiService;
        private readonly IConfiguration _configuration;

        public DepartmentDetailsModel(ApiService apiService, IConfiguration configuration)
        {
            _apiService = apiService;
            _configuration = configuration;
        }

        public DepartmentDto? Department { get; set; }

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

            return Page();
        }
    }
}
