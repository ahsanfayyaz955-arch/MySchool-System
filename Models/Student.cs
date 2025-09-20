namespace MySchool_System.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Age{ get; set; }
        public string RollNumber { get; set; }
        public string Name { get; set; }
        public string Class { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        // Link to registered user
        public string UserId { get; set; }
        public User User { get; set; }

        public ICollection<Result> Results { get; set; }
    }

    public class Result
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public Student Student { get; set; }

        public string Subject { get; set; }
        public int Marks { get; set; }
    }
}
