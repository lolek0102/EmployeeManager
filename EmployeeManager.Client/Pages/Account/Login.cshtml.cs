using EmployeeManager.Application.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployeeManager.Client.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly ApiService _apiService;

        [BindProperty]
        public LoginRequest LoginRequest { get; set; } = new();

        public LoginModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var response = await _apiService.LoginAsync(LoginRequest);

                if (response is null)
                {
                    TempData["ErrorMessage"] = "Invalid login attempt.";
                    return RedirectToPage("/Error");
                }

                // Store the token and user info in cookies
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    Expires = DateTime.UtcNow.AddHours(1)
                };
                Response.Cookies.Append("Token", response.Token, cookieOptions);
                Response.Cookies.Append("Username", response.Username, cookieOptions);
                Response.Cookies.Append("Role", response.Role, cookieOptions);

                return RedirectToPage("/Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
                return RedirectToPage("/Error");
            }
        }
    }
}
