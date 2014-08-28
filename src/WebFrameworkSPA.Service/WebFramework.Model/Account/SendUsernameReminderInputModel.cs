using System.ComponentModel.DataAnnotations;

namespace AdminWeb.Model
{
    public class SendUsernameReminderInputModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}