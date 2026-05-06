using Microsoft.AspNetCore.Mvc;
using DashboardApp.Data;
using System.Linq;

namespace DashboardApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        public HomeController(AppDbContext context) => _context = context;

        public IActionResult Index()
        {
            // Simple view – you can redirect to admin area or show statistics for current user
            return View();
        }
    }
}