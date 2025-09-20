using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySchool_System.Data;
using MySchool_System.Models;
using MySchool_System.ViewModel;
using System.Net.Http.Json;

namespace MySchool_System.Controllers
{
    [Authorize(Roles = "Teacher")]
    public class TeacherController : Controller
    {
        private readonly StudentApiService _studentService;
        private readonly IHttpClientFactory _http;
        private readonly SchoolDbContext _context;

        // ✅ Single constructor with all dependencies
        public TeacherController(StudentApiService studentService, IHttpClientFactory http, SchoolDbContext context)
        {
            _studentService = studentService;
            _http = http;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ManageStudent()
        {
            var client = _http.CreateClient();

            // Ye tumhare API se list fetch karega
            var students = await client.GetFromJsonAsync<List<Student>>("http://localhost:5220/api/students");

            return View(students); // View ko List<Student> bhej diya
        }

        [HttpPost]
        public async Task<IActionResult> Add(Student student)
        {
            var client = _http.CreateClient();
            await client.PostAsJsonAsync("http://localhost:5220/api/students", student);
            return RedirectToAction("ManageStudent");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Student student)
        {
            var client = _http.CreateClient();
            await client.PutAsJsonAsync($"https://localhost:5001/api/students/{student.Id}", student);
            return RedirectToAction("ManageStudent");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var client = _http.CreateClient();
            await client.DeleteAsync($"https://localhost:5001/api/students/{id}");
            return RedirectToAction("ManageStudent");
        }

        // 📌 Add Result (GET form)
        public IActionResult AddResult(int studentId)
        {
            ViewBag.StudentId = studentId;
            return View();
        }

        // 📌 Add Result (POST form)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddResult(Result model)
        {
            if (ModelState.IsValid)
            {
                _context.Results.Add(model);
                _context.SaveChanges();
                return RedirectToAction("ManageStudent"); // 🔹 Redirect to ManageStudent instead of ManageResults
            }
            return View(model);
        }
    }
}
