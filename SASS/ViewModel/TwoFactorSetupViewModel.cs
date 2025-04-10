namespace SASS.ViewModel
{
    public class TwoFactorSetupViewModel
    {
        public string SharedKey { get; set; } = string.Empty;
        public string QRCodeImageUrl { get; set; } = string.Empty;
        public string VerificationCode { get; set; } = string.Empty;
    }
}
