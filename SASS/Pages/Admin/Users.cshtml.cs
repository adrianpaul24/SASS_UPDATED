using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using SASS.Data;
using SASS.Models;

namespace SASS.Pages.Admin
{
    [Authorize(Roles = "admin")] // Restrict access to admins only
    public class UsersModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public UsersModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Users> UsersList { get; set; } = new();

        [BindProperty]
        public Users EditUser { get; set; } = new();

        [BindProperty]
        public string? NewPassword { get; set; }

        public async Task OnGetAsync()
        {
            UsersList = await _context.Users.ToListAsync();
        }

        public async Task<IActionResult> OnPostApproveAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null && user.Role == UserRole.Pending)
            {
                user.Role = UserRole.User;
                user.IsActive = true;
                await _context.SaveChangesAsync();
            }
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostToggleActiveAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                user.IsActive = !user.IsActive;
                await _context.SaveChangesAsync();
            }
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage();
        }
        public async Task<IActionResult> OnPostEditAsync()
        {
            if (EditUser == null)
            {
                return RedirectToPage();
            }

            var user = await _context.Users.FindAsync(EditUser.Id);
            if (user != null)
            {
                user.FirstName = EditUser.FirstName;
                user.LastName = EditUser.LastName;
                user.Username = EditUser.Username;
                user.Email = EditUser.Email;
                user.Phone = EditUser.Phone;
                user.Role = EditUser.Role;
                user.IsActive = EditUser.IsActive;

                // Hash the password only if a new one is provided
                if (!string.IsNullOrEmpty(NewPassword))
                {
                    user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(NewPassword);
                }

                await _context.SaveChangesAsync();
            }

            return RedirectToPage();
        }
    }
}