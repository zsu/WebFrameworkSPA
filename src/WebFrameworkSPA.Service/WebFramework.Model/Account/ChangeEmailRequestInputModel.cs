using System.ComponentModel.DataAnnotations;

namespace AdminWeb.Model
{
    public class ChangeEmailRequestInputModel
    {
        //[Required]
        [EmailAddress]
        public string NewEmail { get; set; }
    }
}