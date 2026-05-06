using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DashboardApp.Data;
using System.Linq;
using System;

namespace DashboardApp.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;
        public AdminController(AppDbContext context) => _context = context;

        // User list showing hidden information (actions, banned details)
        public IActionResult Users()
        {
            var users = _context.Users.Include(u => u.UserActions).ToList();
            return View(users);
        }

        // Detailed view for one user (including actions)
        public IActionResult UserDetail(int id)
        {
            var user = _context.Users.Include(u => u.UserActions).FirstOrDefault(u => u.Id == id);
            if (user == null) return NotFound();
            return View(user);
        }

        // Game list with achievements count and basic stats
        public IActionResult Games()
        {
            var games = _context.Games.Include(g => g.Achievements).ToList();
            return View(games);
        }

        // Per‑game statistics: users on specific days, total sessions, active users now
        public IActionResult GameStats(int id)
        {
            var game = _context.Games.Include(g => g.Achievements).FirstOrDefault(g => g.Id == id);
            if (game == null) return NotFound();

            // Stat 1: total sessions
            ViewBag.TotalSessions = _context.GameSessions.Count(s => s.GameId == id);

            // Stat 2: users currently online (no EndTime)
            ViewBag.CurrentPlayers = _context.GameSessions.Count(s => s.GameId == id && s.EndTime == null);

            // Stat 3: active users per day (last 7 days)
            var last7Days = Enumerable.Range(0, 7).Select(offset => DateTime.Now.Date.AddDays(-offset)).ToList();
            var sessionsLastWeek = _context.GameSessions
                .Where(s => s.GameId == id && s.StartTime >= DateTime.Now.AddDays(-7))
                .AsEnumerable()
                .GroupBy(s => s.StartTime.Date)
                .Select(g => new { Date = g.Key, Count = g.Select(x => x.UserId).Distinct().Count() })
                .ToList();
            ViewBag.DailyUsers = last7Days.Select(d => new {
                Date = d.ToString("dd/MM"),
                Users = sessionsLastWeek.FirstOrDefault(x => x.Date == d)?.Count ?? 0
            }).ToList();

            return View(game);
        }
    }
}