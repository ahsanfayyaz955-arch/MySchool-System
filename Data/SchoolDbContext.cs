using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MySchool_System.Models;
using MySchool_System.ViewModel;

namespace MySchool_System.Data
{
    public class SchoolDbContext : IdentityDbContext<User>
    {
        public SchoolDbContext(DbContextOptions<SchoolDbContext> options): base(options)
        {
          
        }
        public DbSet<Student> Students { get; set; }
        public DbSet<Result> Results { get; set; }



    }
      
    
}
