using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace DashboardApp.Controllers
{
    public class HomeController : Controller
    {
        [AllowAnonymous]
        public IActionResult Index()
        {
            if (User.Identity?.IsAuthenticated ?? false)
            {
                // redirect players to their own stats, others to their respective pages
                if (User.IsInRole("admin"))
                    return RedirectToAction("Index", "Admin");
                if (User.IsInRole("supervisor"))
                    return RedirectToAction("Index", "Supervisor");
                return RedirectToAction("MyStats", "Player");
            }
            return View();
        }
    }
}