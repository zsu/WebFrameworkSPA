﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class PasswordResetModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}