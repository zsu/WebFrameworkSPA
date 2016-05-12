using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class PasswordResetModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}