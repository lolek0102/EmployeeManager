using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployeeManager.Client.Pages.Account
{
    public class LogoutModel : PageModel
    {
        public IActionResult OnGet()
        {
            // Clear the cookies
            Response.Cookies.Delete("Token");
            Response.Cookies.Delete("Username");
            Response.Cookies.Delete("Role");

            // Redirect to login page or home page
            return RedirectToPage("/Account/Login");
        }
    }
}
