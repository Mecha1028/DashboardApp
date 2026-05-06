using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DashboardApp.Data;
using System.Linq;

namespace DashboardApp.Controllers
{
    [Authorize(Roles = "supervisor,admin")]     // admin can also use this
    public class SupervisorController : Controller
    {
        private readonly AppDbContext _context;
        public SupervisorController(AppDbContext context) => _context = context;

        public IActionResult Users()
        {
            var users = _context.Users.ToList();   // no actions/bans loaded
            // Optionally hide banned status? Brief says only hidden from non-admins.
            return View(users);
        }
    }
}