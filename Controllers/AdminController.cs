using DashboardApp.Data;
using DashboardApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;

namespace DashboardApp.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;
        public AdminController(AppDbContext context) => _context = context;

        // ---------- Dashboard ----------
        public IActionResult Index()
        {
            ViewBag.Reports = _context.Reports
                .Include(r => r.Reporter)
                .Include(r => r.ReportedUser)
                .ToList();
            return View();
        }

        // ---------- Ban a player ----------
        public IActionResult Ban()
        {
            ViewBag.Players = _context.Users.Where(u => u.PermissionLevel == "player").ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Ban(int userId, int days, string reason)
        {
            var user = _context.Users.Find(userId);
            if (user == null) return NotFound();
            user.Banned = true;
            user.BanEnd = DateTime.Now.AddDays(days);

            var adminId = int.Parse(User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier)!);
            _context.UserActions.Add(new UserAction
            {
                UserId = userId,
                ActionType = "BAN",
                Description = $"{reason} (duration: {days} days)",
                AdminId = adminId,
                Timestamp = DateTime.Now
            });
            _context.SaveChanges();
            return RedirectToAction("Bans");
        }

        // ---------- Unban ----------
        [HttpPost]
        public IActionResult Unban(int userId)
        {
            var user = _context.Users.Find(userId);
            if (user == null) return NotFound();
            user.Banned = false;
            user.BanEnd = null;

            var adminId = int.Parse(User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier)!);
            _context.UserActions.Add(new UserAction
            {
                UserId = userId,
                ActionType = "UNBAN",
                Description = "Ban lifted by admin",
                AdminId = adminId,
                Timestamp = DateTime.Now
            });
            _context.SaveChanges();
            return RedirectToAction("Bans");
        }

        // ---------- Bans list (with reasons) ----------
        public IActionResult Bans()
        {
            var bannedUsers = _context.Users.Where(u => u.Banned).ToList();

            // Most recent ban reason for each user
            var reasons = _context.UserActions
                .Where(a => a.ActionType == "BAN")
                .GroupBy(a => a.UserId)
                .Select(g => g.OrderByDescending(a => a.Timestamp).First())
                .ToList();

            ViewBag.Reasons = reasons;
            return View(bannedUsers);
        }

        // ---------- Hidden actions for a user ----------
        public IActionResult UserActions(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null) return NotFound();
            ViewBag.Actions = _context.UserActions
                .Where(a => a.UserId == id)
                .Include(a => a.Admin)
                .ToList();
            return View(user);
        }

        // ---------- Dismiss a report ----------
        [HttpPost]
        public IActionResult DismissReport(int reportId)
        {
            var report = _context.Reports.Find(reportId);
            if (report != null)
            {
                _context.Reports.Remove(report);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}