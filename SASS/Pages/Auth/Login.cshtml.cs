using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SASS.Data;
using SASS.Models;
using BCrypt.Net;
using System.Linq;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace SASS.Pages.Auth
{
    public class LoginModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public LoginModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string? LoginInput { get; set; }

        [BindProperty]
        public string? Password { get; set; }

        public string? ErrorMessage { get; set; }

        public void OnGet()
        {
            ErrorMessage = null;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrEmpty(LoginInput) || string.IsNullOrEmpty(Password))
            {
                ErrorMessage = "Username/Email and Password are required.";
                return Page();
            }

            Console.WriteLine($"LoginInput: {LoginInput}");

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == LoginInput || u.Email == LoginInput);

            if (user == null)
            {
                ErrorMessage = "Invalid username/email or password.";
                return Page();
            }

            if (!BCrypt.Net.BCrypt.Verify(Password, user.PasswordHash))
            {
                ErrorMessage = "Invalid username/email or password.";
                return Page();
            }

            if (user.Role == UserRole.Pending)
            {
                ErrorMessage = "Your account is pending approval.";
                return Page();
            }

            if (!user.IsActive)
            {
                ErrorMessage = "Your account is deactivated.";
                return Page();
            }

            await SignInUser(user);

            var twoFactor = await _context.UserTwoFactor
                .FirstOrDefaultAsync(t => t.UserId == user.Id && t.IsEnabled == true);

            if (twoFactor == null)
            {
                HttpContext.Session.SetInt32("2FA_UserId", user.Id);
                return RedirectToPage("/Auth/Enable2FA");
            }

            HttpContext.Session.SetInt32("2FA_UserId", user.Id);
            return RedirectToPage("/Auth/Verify2FA");
        }

        private async Task SignInUser(Users user)
        {
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.Username),
        new Claim("FirstName", user.FirstName),
        new Claim("LastName", user.LastName),
        new Claim(ClaimTypes.Role, user.Role.ToString().ToLower())
    };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }

    }
}
