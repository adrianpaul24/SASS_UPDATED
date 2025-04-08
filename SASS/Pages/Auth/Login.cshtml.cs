using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SASS.Data; // Import your database context
using SASS.Models; // Import your User model
using BCrypt.Net; // Ensure BCrypt is used for password verification
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

        public LoginModel(ApplicationDbContext context) // Inject Database Context
        {
            _context = context;
        }

        [BindProperty]
        public string? LoginInput { get; set; } // Accepts either Username or Email

        [BindProperty]
        public string? Password { get; set; }

        public string? ErrorMessage { get; set; } // Error message for invalid login

        public void OnGet()
        {
            ErrorMessage = null; // Reset error message on page load
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrEmpty(LoginInput) || string.IsNullOrEmpty(Password))
            {
                ErrorMessage = "Username/Email and Password are required.";
                return Page();
            }

            // Find user by either Username or Email
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == LoginInput || u.Email == LoginInput);

            if (user == null || !BCrypt.Net.BCrypt.Verify(Password, user.PasswordHash))
            {
                ErrorMessage = "Invalid username/email or password.";
                return Page();
            }

            if (user.Role == UserRole.Pending)
            {
                ErrorMessage = "Your account is pending approval. Please wait for an admin to activate it.";
                return Page();
            }

            if (!user.IsActive)
            {
                ErrorMessage = "Your account is deactivated. Please contact the administrator.";
                return Page();
            }

            // Authentication logic
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim("FirstName", user.FirstName),
                new Claim("LastName", user.LastName),
                new Claim(ClaimTypes.Role, user.Role.ToString().ToLower())
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

            return RedirectToPage("/Dashboard/Index");
        }
    }
}