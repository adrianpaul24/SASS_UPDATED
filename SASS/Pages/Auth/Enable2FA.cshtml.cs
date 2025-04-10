using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QRCoder;
using System.Security.Cryptography;
using System.Text;
using OtpNet; // Make sure OtpNet is installed
using SASS.Models;
using SASS.Data;
using SASS.ViewModel;
using Microsoft.AspNetCore.Authorization;
using SASS.Model;

namespace SASS.Pages.Auth
{
    public class Enable2FAModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public Enable2FAModel(ApplicationDbContext db)
        {
            _db = db;
        }

        [BindProperty]
        public TwoFactorSetupViewModel TwoFactor { get; set; }

        public IActionResult OnGet()
        {
            var user = GetCurrentUser();
            if (user == null)
                return RedirectToPage("/Auth/Login");

            var secret = GenerateSecretKey();
            var appName = "SmartSchedulingSystem";

            string totpUri = $"otpauth://totp/{appName}:{user.Email}?secret={secret}&issuer={appName}";
            string qrCodeUrl = GenerateQRCode(totpUri);

            TwoFactor = new TwoFactorSetupViewModel
            {
                SharedKey = secret,
                QRCodeImageUrl = qrCodeUrl
            };

            return Page();
        }

        public IActionResult OnPost()
        {
            var user = GetCurrentUser();
            if (user == null)
                return RedirectToPage("/Auth/Login");

            if (!IsValidCode(TwoFactor.SharedKey, TwoFactor.VerificationCode))
            {
                ModelState.AddModelError(string.Empty, "Invalid verification code.");
                return Page();
            }

            var existing = _db.UserTwoFactor.FirstOrDefault(x => x.UserId == user.Id);
            if (existing != null)
            {
                existing.SecretKey = TwoFactor.SharedKey;
                existing.IsEnabled = true;  // Set 2FA as enabled
            }
            else
            {
                _db.UserTwoFactor.Add(new UserTwoFactor
                {
                    UserId = user.Id,
                    SecretKey = TwoFactor.SharedKey,
                    IsEnabled = true
                });
            }

            _db.SaveChanges(); // Save to the database
            return RedirectToPage("/Auth/Success2FA");
        }


        private string GenerateSecretKey()
        {
            byte[] bytes = new byte[20];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(bytes);
            }
            return Base32Encoding.ToString(bytes); // OtpNet's Base32
        }

        private string GenerateQRCode(string uri)
        {
            var qrGenerator = new QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode(uri, QRCodeGenerator.ECCLevel.Q);
            var qrCode = new Base64QRCode(qrCodeData);
            return "data:image/png;base64," + qrCode.GetGraphic(10);
        }

        private bool IsValidCode(string secret, string code)
        {
            var totp = new Totp(Base32Encoding.ToBytes(secret));
            return totp.VerifyTotp(code, out _, new VerificationWindow(2, 2));
        }

        private Users GetCurrentUser()
        {
            var username = User.Identity?.Name;
            return _db.Users.FirstOrDefault(u => u.Username == username);
        }
    }
}
