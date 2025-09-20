using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MySchool_System.Models;

namespace MySchool_System.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult DownloadPdf(int id)
        {
            string apiUrl = $"https://localhost:5001/api/students/pdf/{id}";
            return Redirect(apiUrl);
        }// direct download
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
