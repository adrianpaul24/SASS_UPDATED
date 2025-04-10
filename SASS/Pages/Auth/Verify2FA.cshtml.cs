using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SASS.Data;
using SASS.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using OtpNet;

namespace SASS.Pages.Auth
{
    public class Verify2FAModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public Verify2FAModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string OTP { get; set; }

        public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = HttpContext.Session.GetInt32("2FA_UserId");

            if (userId == null)
                return RedirectToPage("/Auth/Login");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var userId = HttpContext.Session.GetInt32("2FA_UserId");

            if (userId == null)
                return RedirectToPage("/Auth/Login");

            var user = await _context.Users.FindAsync(userId);
            var twoFactor = await _context.UserTwoFactor.FirstOrDefaultAsync(t => t.UserId == userId);

            if (user == null || twoFactor == null || string.IsNullOrEmpty(twoFactor.SecretKey))
            {
                ErrorMessage = "Unable to verify 2FA.";
                return Page();
            }

            if (!VerifyOTP(twoFactor.SecretKey, OTP))
            {
                ErrorMessage = "Invalid OTP.";
                return Page();
            }

            // OTP is correct → clear session and sign user in
            HttpContext.Session.Remove("2FA_UserId");
            await SignInUser(user);

            return RedirectToPage("/Dashboard/Index");
        }

        // Fixed method: Removed duplicate
        private bool VerifyOTP(string secret, string otp)
        {
            try
            {
                var otpKey = Base32Encoding.ToBytes(secret);
                var totp = new Totp(otpKey);
                return totp.VerifyTotp(otp, out long timeStepMatched, VerificationWindow.RfcSpecifiedNetworkDelay);
            }
            catch
            {
                return false;
            }
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
