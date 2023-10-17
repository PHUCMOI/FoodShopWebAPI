﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models_Layer.ModelRequest
{
    public class UserRequest
    {
        public int UserId { get; set; }

        public string? UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [RegularExpression(@"^(?=.*[a-zA-Z])(?=.*\d).{8,}$", ErrorMessage = "Password must contain at least one letter and one number , and has a minimum length of 8 characters")]
        public string? Password { get; set; }

        public string? Role { get; set; }

        [RegularExpression(@"^0\d{9}$|^\d{10}$", ErrorMessage = "Phone number must have 10 digits.")]
        public string? PhoneNumber { get; set; }
        public string? Status { get; set; }
        public string? AccessToken { get; set; }
    }
}
