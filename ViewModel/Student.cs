using System.ComponentModel.DataAnnotations;

namespace MySchool_System.ViewModel
{
    public class Student
    {
        public class Students
        {
            [Key]
            public int Id { get; set; }

            [Required]
            public string RollNo { get; set; }

            [Required]
            public string Name { get; set; }

            [Required]
            public string Class { get; set; }

            [Required]
            public int Age { get; set; }
        }

    }
}
