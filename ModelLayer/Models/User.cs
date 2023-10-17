using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fooding_Shop.Models;

public partial class User
{
    public int UserId { get; set; }
    [Required(ErrorMessage = "Username is required")]
    public string? UserName { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [RegularExpression(@"^(?=.*[a-zA-Z])(?=.*\d).{8,}$", ErrorMessage = "Password must contain at least one letter and one number , and has a minimum length of 8 characters")]
    public string? Password { get; set; }
    [NotMapped]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; }

    public string? Role { get; set; }

    [Required(ErrorMessage = "Phone number is required")]
    [RegularExpression(@"^(0\d{9}|\+84\d{9})$", ErrorMessage = "Invalid phone number")]
    public string? PhoneNumber { get; set; }

    public string? Status { get; set; }

    public int? CreateBy { get; set; }

    public DateTime? CreateDate { get; set; }

    public int? UpdateBy { get; set; }

    public DateTime? UpdateDate { get; set; }

    public bool? IsDeleted { get; set; }
}
