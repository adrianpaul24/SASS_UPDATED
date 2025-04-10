using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using SASS.Data;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace SASS.Pages.Dashboard
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public IndexModel(ApplicationDbContext db)
        {
            _db = db;
        }

        public bool Is2FAEnabled { get; set; }

        public void OnGet()
        {
            var username = User.Identity?.Name;
            var user = _db.Users.FirstOrDefault(u => u.Username == username);
            if (user != null)
            {
                var twoFA = _db.UserTwoFactor.FirstOrDefault(t => t.UserId == user.Id);
                Is2FAEnabled = twoFA?.IsEnabled == true;
            }
        }
    }
}
