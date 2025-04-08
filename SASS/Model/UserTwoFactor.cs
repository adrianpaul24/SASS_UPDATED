using System.ComponentModel.DataAnnotations.Schema;

namespace SASS.Model
{
    [Table("user_two_factor")]
    public class UserTwoFactor
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string SecretKey { get; set; } = string.Empty;
        public bool IsEnabled { get; set; }
    }
}
