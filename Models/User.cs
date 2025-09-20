using Microsoft.AspNetCore.Identity;
namespace MySchool_System.Models
{
    public class User:IdentityUser 
    {
        public string RollNumber { get; set; }
        public string Class { get; set; }
        public int Age { get; set; }
    }
}
