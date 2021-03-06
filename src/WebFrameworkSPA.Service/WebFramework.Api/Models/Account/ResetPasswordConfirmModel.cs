﻿using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class ResetPasswordConfirmModel
    {
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Password confirmation must match password.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        public string Key { get; set; }
    }
}