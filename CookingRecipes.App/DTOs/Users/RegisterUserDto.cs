using System.ComponentModel.DataAnnotations;

namespace CookingRecipes.App.DTOs.Users
{
    public class RegisterUserDto
    {
        [Required(ErrorMessage = "First Name is required.")]
        [Display(Name = "First name")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "Last Name is required")]
        [Display(Name = "Last name")]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessage = "Date of Birth is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "Date of birth")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [Display(Name = "Email")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Password is required.")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d).{6,}$",
            ErrorMessage = "Password must be at least 6 characters, contain at least one uppercase letter, and one number.")]
        [Display(Name = "Password")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Please confirm your password.")]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        [Display(Name = "Confirm password")]
        public string ConfirmPassword { get; set; } = null!;
    }
}
