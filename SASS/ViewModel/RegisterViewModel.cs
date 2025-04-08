using System.ComponentModel.DataAnnotations;

namespace SASS.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Username { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        [Required, Compare("Password", ErrorMessage = "Passwords do not match"), DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
