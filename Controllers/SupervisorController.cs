using DashboardApp.Data;
using DashboardApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;

namespace DashboardApp.Controllers
{
    [Authorize(Roles = "supervisor,admin")]
    public class SupervisorController : Controller
    {
        private readonly AppDbContext _context;
        public SupervisorController(AppDbContext context) => _context = context;

        // View list of all players (ID, username) to select one and view their stats
        public IActionResult Index()
        {
            var players = _context.Users.Where(u => u.PermissionLevel == "player").ToList();
            return View(players);
        }

        // View a specific player's stats (highscores, achievements)
        public IActionResult PlayerStats(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null) return NotFound();
            var highscores = _context.Highscores.Include(h => h.Game).Where(h => h.UserId == id).ToList();
            var achievements = _context.UserAchievements
                .Include(a => a.Achievement).ThenInclude(a => a.Game)
                .Where(a => a.UserId == id).ToList();
            ViewBag.Highscores = highscores;
            ViewBag.Achievements = achievements;
            return View(user);
        }

        // Report form (GET) – select player and game from dropdown
        public IActionResult Report()
        {
            ViewBag.Players = _context.Users.Where(u => u.PermissionLevel == "player").ToList();
            ViewBag.Games = _context.Games.ToList();
            return View();
        }

        // Handle report submission
        [HttpPost]
        public IActionResult Report(int reportedUserId, int gameId, string reason)
        {
            var reporterId = int.Parse(User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier)!);
            var report = new Report
            {
                ReporterId = reporterId,
                ReportedUserId = reportedUserId,
                GameId = gameId,
                Reason = reason ?? "No reason provided"
            };
            _context.Reports.Add(report);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        // Game statistics (same as before but now supervisor can access)
        public IActionResult GameStats(int gameId)
        {
            var game = _context.Games.Include(g => g.Highscores).FirstOrDefault(g => g.Id == gameId);
            if (game == null) return NotFound();
            ViewBag.TotalPlayers = game.Highscores.Select(h => h.UserId).Distinct().Count();
            ViewBag.HighestScore = game.Highscores.Max(h => (int?)h.Score) ?? 0;
            ViewBag.AverageScore = game.Highscores.Average(h => (double?)h.Score) ?? 0;
            return View(game);
        }
    }
}