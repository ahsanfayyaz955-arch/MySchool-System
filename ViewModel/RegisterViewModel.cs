using System.ComponentModel;
using System.ComponentModel.DataAnnotations;



namespace MySchool_System.ViewModel
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Please enter your name")]
        [DisplayName("Full Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter your contact email")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [DisplayName("Email ID")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter your Roll Number")]
        [DisplayName("Roll Number")]
        public string RollNumber { get; set; }

        [Required(ErrorMessage = "Please select your class")]
        [DisplayName("Class")]
        public string Class { get; set; }  // Example: "9th", "10th"

        [Required(ErrorMessage = "Please enter your age")]
        [Range(5, 100, ErrorMessage = "Please enter a valid age")]
        [DisplayName("Age")]
        public int Age { get; set; }

        [Required]
        [StringLength(40, MinimumLength = 8, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long")]
        [DataType(DataType.Password)]
        [DisplayName("Password")]
        [Compare("ConfirmPassword", ErrorMessage = "Password does not match")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please confirm your password")]
        [DataType(DataType.Password)]
        [DisplayName("Confirm Password")]
        public string ConfirmPassword { get; set; }
    }
}
