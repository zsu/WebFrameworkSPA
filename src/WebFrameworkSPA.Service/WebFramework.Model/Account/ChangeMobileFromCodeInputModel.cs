using System.ComponentModel.DataAnnotations;

namespace AdminWeb.Model
{
    public class ChangeMobileFromCodeInputModel
    {
        [Required]
        public string Code { get; set; }
    }
    
}