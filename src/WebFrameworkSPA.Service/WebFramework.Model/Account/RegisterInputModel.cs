using System.ComponentModel.DataAnnotations;

namespace AdminWeb.Model
{
    public class RegisterInputModel
    {
        [ScaffoldColumn(false)]
        [Required]
        public string Username { get; set; }
        
        //[Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        [Required]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage="Password confirmation must match password.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}