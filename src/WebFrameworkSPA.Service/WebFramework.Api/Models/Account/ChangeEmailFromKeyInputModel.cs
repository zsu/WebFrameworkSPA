using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

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