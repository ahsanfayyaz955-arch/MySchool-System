using Azure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MySchool_System.EmailSender;
using MySchool_System.Models;
using MySchool_System.Services;
using MySchool_System.ViewModel;
using System.Net.WebSockets;

namespace MySchool_System.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<User> _userManager;

        public AdminController(RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            this.roleManager = roleManager;
            _userManager = userManager;
        }

        public IActionResult Home()
        {
            return View();
        }
        public IActionResult AdminDashboard()
        {
            return View();
        }

  
        public async Task<IActionResult>RoleList()
        {
            var roles = await  roleManager.Roles.ToListAsync();
            return View(roles);
        }

        // GET: Add Role form
        [HttpGet]
        public IActionResult AddRoll()
        {
            return View();
        }

        // POST: Add Role
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRole(string roleName)
        {
            // 1️⃣ Validation
            if (string.IsNullOrWhiteSpace(roleName))
            {
                ModelState.AddModelError("", "Role name cannot be empty");
                return View();
            }

            // 2️⃣ Already exists check
            if (await roleManager .RoleExistsAsync(roleName))
            {
                ModelState.AddModelError("", "Role already exists");
                return View();
            }

            // 3️⃣ Create new role
            var result = await roleManager .CreateAsync(new IdentityRole(roleName.Trim()));
            if (result.Succeeded)
            {
                TempData["Message"] = $"Role '{roleName}' created successfully";
                return RedirectToAction("RoleList"); // List of roles page
            }

            // 4️⃣ Agar errors aaye to ModelState me add karo
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View();
        }

        // GET: Confirm delete role
        [HttpGet]
        public async Task<IActionResult> DeleteRole(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("RoleList");
            }

            var role = await roleManager.FindByIdAsync(id);
            if (role == null)
            {
                TempData["Success"] = "Role not found!";
                return RedirectToAction("RoleList");
            }

            return View(role); // Confirm page
        }

        // POST: Actually delete role
        [HttpPost, ActionName("DeleteRole")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteRoleConfirmed(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            if (role == null)
            {
                TempData["Success"] = "Role not found!";
                return RedirectToAction("RoleList");
            }

            var result = await roleManager .DeleteAsync(role);
            if (result.Succeeded)
            {
                TempData["Success"] = $"Role '{role.Name}' deleted successfully.";
            }
            else
            {
                TempData["Success"] = string.Join("; ", result.Errors.Select(e => e.Description));
            }

            return RedirectToAction("RoleList");
        }

        // GET: Edit Role form
        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("RoleList");
            }

            var role = await roleManager .FindByIdAsync(id);
            if (role == null)
            {
                TempData["Message"] = "Role not found!";
                return RedirectToAction("RoleList");
            }

            return View(role);
        }

        // POST: Update Role
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRole(string id, string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
            {
                ModelState.AddModelError("", "Role name cannot be empty");
                return View(await roleManager .FindByIdAsync(id));
            }

            var role = await roleManager .FindByIdAsync(id);
            if (role == null)
            {
                TempData["Message"] = "Role not found!";
                return RedirectToAction("RoleList");
            }

            role.Name = roleName;
            role.NormalizedName = roleName.ToUpper();

            var result = await roleManager .UpdateAsync(role);
            if (result.Succeeded)
            {
                TempData["Message"] = $"Role '{roleName}' updated successfully.";
                return RedirectToAction("RoleList");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(role);
        }

        public async Task <IActionResult> UserList()
        {
            var users = await _userManager.Users.ToListAsync();
            return View(users);
        }
        [HttpGet]
        public async Task<IActionResult> AddUser()
        {
            var roles = new SelectList(await roleManager.Roles.ToListAsync());
            ViewBag.Roles = roles;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(UserViewModel user)
        {
            if (ModelState.IsValid)
            {
                // check user already exist?
                var urs = await _userManager.FindByEmailAsync(user.Email);

                if (urs == null) // ✅ sirf tab new user create hoga jab email exist nahi karta
                {
                    string password = PasswordService.GetPassword();

                    var newUser = new User()
                    {
                        UserName = user.Email,
                        Email = user.Email,
                        Class = "N/A", // ✅ default value
                        RollNumber = "N/A" // ✅ default value
                    };

                    // ✅ Create user with password
                    var createResult = await _userManager.CreateAsync(newUser, password);

                    if (createResult.Succeeded)
                    {
                        // assign role
                        var roleResult = await _userManager.AddToRoleAsync(newUser, user.Role);
                        if (roleResult.Succeeded)
                        {
                            // send email
                            string message = $@"Dear User <br/><br/>
                        Your login account has been created.<br/><br/>
                        User Id: {user.Email}<br/>
                        Password: {password}<br/>
                        Role: {user.Role}<br/><br/>
                        Regards,<br/>User Admin<br/>MY SCHOOL SYSTEM";

                            string subject = "Login Information";
                            EmailSender.EmailSender.SendMail(user.Email, subject, message); // ✅ args ka order sahi karo (subject pehle, phir body)

                            TempData["Message"] = "User successfully added.";
                            return RedirectToAction("UserList");
                        }
                        else
                        {
                            TempData["Message"] = "User created, but role not assigned.";
                        }
                    }
                    else
                    {
                        TempData["Message"] = "Unable to create user: " +
                            string.Join(", ", createResult.Errors.Select(e => e.Description));
                    }
                }
                else
                {
                    TempData["Message"] = "This email is already registered!";
                }
            }

            // agar error ho to roles wapas load karna hai warna dropdown empty
            ViewBag.Roles = new SelectList(await roleManager.Roles.ToListAsync());
            return View(user);
        }

    }



}


