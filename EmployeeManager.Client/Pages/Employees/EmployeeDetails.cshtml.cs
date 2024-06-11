using EmployeeManager.Application.Dtos;
using EmployeeManager.Client.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployeeManager.Client.Pages.Employees
{
    public class EmployeeDetailsModel : PageModel
    {
        private readonly ApiService _apiService;
        private readonly IConfiguration _configuration;

        public EmployeeDetailsModel(ApiService apiService, IConfiguration configuration)
        {
            _apiService = apiService;
            _configuration = configuration;
        }

        public EmployeeDto? Employee { get; set; }

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

            return Page();
        }
    }
}
