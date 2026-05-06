using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DashboardApp.Data;
using System.Linq;
using System.Security.Claims;

namespace DashboardApp.Controllers
{
    [Authorize(Roles = "player,supervisor,admin")]      // all logged-in users
    public class PlayerController : Controller
    {
        private readonly AppDbContext _context;
        public PlayerController(AppDbContext context) => _context = context;

        private int CurrentUserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        // My personal stats
        public IActionResult MyStats()
        {
            var userId = CurrentUserId;
            var highscores = _context.Highscores
                .Include(h => h.Game)
                .Where(h => h.UserId == userId)
                .ToList();
            var achievements = _context.UserAchievements
                .Include(a => a.Achievement).ThenInclude(a => a.Game)
                .Where(a => a.UserId == userId)
                .ToList();
            var user = _context.Users.Find(userId);
            ViewBag.User = user;
            ViewBag.Highscores = highscores;
            ViewBag.Achievements = achievements;
            return View();
        }

        // Leaderboard for a specific game (gameId passed as query)
        public IActionResult Leaderboard(int gameId)
        {
            var game = _context.Games.Find(gameId);
            if (game == null) return NotFound();
            var leaderboard = _context.Highscores
                .Include(h => h.User)
                .Where(h => h.GameId == gameId)
                .OrderByDescending(h => h.Score)
                .ToList();
            ViewBag.Game = game;
            return View(leaderboard);
        }

        // Public list of games with links to leaderboard
        public IActionResult Games()
        {
            var games = _context.Games.ToList();
            return View(games);
        }
    }
}