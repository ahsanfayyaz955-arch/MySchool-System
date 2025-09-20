using System.ComponentModel.DataAnnotations;

namespace MySchool_System.ViewModel
{
    public class UserViewModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string  Role { get; set; }
    }
}
