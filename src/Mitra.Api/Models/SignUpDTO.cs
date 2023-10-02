using System.ComponentModel.DataAnnotations;

namespace Mitra.Api.Models;

public class SignUpDTO
{
    public string FirstName { get; set; }
    public string LastName { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress]
    public string Email { get; set; }
    public string Country { get; set; }

    [Required(ErrorMessage = "UserName is required")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; }
    public string PhoneNumber { get; set; }

    public string IpAddress { get; set; }
}