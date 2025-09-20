using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MySchool_System.ViewModel
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Please enter your email")]
        [EmailAddress(ErrorMessage = "Please check your email format")]
        [DisplayName("Email id")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DisplayName("Remember me")]
        public bool RememberMe { get; set; }


    }
}
