using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class ChangeEmailFromKeyInputModel
    {
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string Key { get; set; }
    }
}