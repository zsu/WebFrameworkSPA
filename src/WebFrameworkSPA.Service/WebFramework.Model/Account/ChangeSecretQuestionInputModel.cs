using BrockAllen.MembershipReboot;
using System.ComponentModel.DataAnnotations;

namespace AdminWeb.Model
{
    public class PasswordResetSecretsViewModel
    {
        public PasswordResetSecret[] Secrets { get; set; }
    }

    public class AddSecretQuestionInputModel
    {
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        public string Question { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Answer { get; set; }
    }
}