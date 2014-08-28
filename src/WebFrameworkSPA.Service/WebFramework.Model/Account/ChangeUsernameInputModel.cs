using System.ComponentModel.DataAnnotations;

namespace AdminWeb.Model
{
    public class ChangeUsernameInputModel
    {
        [Required]
        public string NewUsername { get; set; }
    }
}