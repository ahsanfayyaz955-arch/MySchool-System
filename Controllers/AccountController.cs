using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySchool_System.Models;
using MySchool_System.ViewModel;
using MySchool_System.Data;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MySchool_System.Models;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;


namespace MySchool_System.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SchoolDbContext  _context;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager, SchoolDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;
        }

        private string GenerateRollNumber()
        {
            // simple timestamp + short guid to reduce collisions
            return $"STU{DateTime.UtcNow:yyyyMMddHHmmssfff}{Guid.NewGuid().ToString("N").Substring(0, 4).ToUpper()}";
        }

        [HttpGet ]
        public IActionResult Register()
        {
            return View();
        }
        // 🔹 Register Page
        public async Task<IActionResult> Register(Student model)
        {
            if (ModelState.IsValid)
            {
                var user = new User { UserName = model.Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // Role assign
                    await _userManager.AddToRoleAsync(user, "Student");

                    // Student table me bhi record banao
                    var student = new Student
                    {
                        Name = model.Name,
                        RollNumber = GenerateRollNumber(),
                        Class = model.Class,
                        Age = model.Age ,
                        UserId = user.Id   // Identity User se link
                    };

                    _context.Students.Add(student);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Index", "Home");
                }
            }
            return View(model);
        }














        // 🔹 Login (GET)
        public IActionResult Login()
        {
            return View();
        }

        // 🔹 Login (POST)
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage); // Debug window me dekhna
                }
                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(
                        user,
                        model.Password,
                        isPersistent: false,
                        lockoutOnFailure: false
                    );

                    if (result.Succeeded)
                    {
                        var roles = await _userManager.GetRolesAsync(user);

                        if (roles.Contains("Admin"))
                            return RedirectToAction("AdminDashboard", "Admin");

                        if (roles.Contains("Teacher"))
                            return RedirectToAction("Index", "Teacher");
                        

                        return RedirectToAction("Index", "Home");
                    }

                    TempData["Message"] = "❌ Password is incorrect.";
                    return View(model);
                }

                TempData["Message"] = "❌ User not found.";
                return View(model);
            }

            TempData["Message"] = "⚠️ Please fill all required fields.";
            return View(model);
        }

        // 🔹 Logout
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
