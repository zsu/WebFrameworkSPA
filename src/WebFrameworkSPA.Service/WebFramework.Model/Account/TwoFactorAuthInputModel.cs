using System.ComponentModel.DataAnnotations;

namespace AdminWeb.Model
{
    public class TwoFactorAuthInputModel
    {
        [Required]
        public string Code { get; set; }

        public string ReturnUrl { get; set; }
    }
}