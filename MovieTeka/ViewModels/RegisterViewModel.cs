using System.ComponentModel.DataAnnotations;

namespace MovieTeka.ViewModels;

public class RegisterViewModel
{
    [Required]
    public string Username { get; set; }
    [Display(Name = "Email address")]
    [Required(ErrorMessage = "Email address is required")]
    public string EmailAddress { get; set; }
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    [Display(Name = "Confirm password")]
    [Required(ErrorMessage = "You've got to confirm password")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Passwords do not match")]
    public string ConfirmPassword { get; set; }
}