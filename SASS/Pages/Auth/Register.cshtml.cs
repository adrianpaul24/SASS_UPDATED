using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SASS.Data;
using SASS.ViewModels;
using SASS.Models;
using BCrypt.Net;

namespace SASS.Pages.Auth
{
    public class RegisterModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public RegisterModel(ApplicationDbContext db)
        {
            _db = db;
        }

        [BindProperty]
        public RegisterViewModel Input { get; set; }

        public string ErrorMessage { get; set; }

        public void OnGet()
        {
            ErrorMessage = null;
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Check if username or email already exists
            if (_db.Users.Any(u => u.Username == Input.Username))
            {
                ErrorMessage = "Username already taken.";
                return Page();
            }
            if (_db.Users.Any(u => u.Email == Input.Email))
            {
                ErrorMessage = "Email already registered.";
                return Page();
            }

            // Hash the password
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(Input.Password);

            // Create new user
            var newUser = new Users
            {
                FirstName = Input.FirstName,
                LastName = Input.LastName,
                Username = Input.Username,
                Email = Input.Email,
                Phone = Input.Phone,
                PasswordHash = hashedPassword,
                Role = Enum.Parse<UserRole>("Pending"),
                IsActive = true
            };

            _db.Users.Add(newUser);
            _db.SaveChanges();

            return RedirectToPage("/Auth/Login");
        }
    }
}
