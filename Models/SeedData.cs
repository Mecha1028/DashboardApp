using System;
using System.Linq;
using DashboardApp.Models;

namespace DashboardApp.Data
{
    public static class SeedData
    {
        public static void Initialize(AppDbContext context)
        {
            if (context.Users.Any()) return;

            static string HashPassword(string pwd) =>
                Convert.ToBase64String(
                    System.Security.Cryptography.SHA256.HashData(
                        System.Text.Encoding.UTF8.GetBytes(pwd)));

            // Users
            var admin = new User
            {
                Username = "admin",
                PasswordHash = HashPassword("admin123"),
                PermissionLevel = "admin",
                CreatedAt = DateTime.Now
            };
            var supervisor = new User
            {
                Username = "supervisor",
                PasswordHash = HashPassword("super123"),
                PermissionLevel = "supervisor",
                CreatedAt = DateTime.Now
            };
            var player1 = new User
            {
                Username = "player1",
                PasswordHash = HashPassword("player123"),
                PermissionLevel = "player",
                CreatedAt = DateTime.Now
            };
            var player2 = new User
            {
                Username = "player2",
                PasswordHash = HashPassword("player456"),
                PermissionLevel = "player",
                CreatedAt = DateTime.Now
            };
            var badplayer = new User
            {
                Username = "badplayer",
                PasswordHash = HashPassword("bad123"),
                PermissionLevel = "player",
                Banned = true,
                BanEnd = DateTime.Now.AddHours(-1),
                CreatedAt = DateTime.Now
            };
            context.Users.AddRange(admin, supervisor, player1, player2, badplayer);
            context.SaveChanges();

            // Ban action (hidden)
            context.UserActions.Add(new UserAction
            {
                UserId = badplayer.Id,
                ActionType = "BAN",
                Description = "Cheating - banned for 7 days",
                AdminId = admin.Id,
                Timestamp = DateTime.Now.AddDays(-1)
            });
            context.SaveChanges();

            // Games
            var game1 = new Game { Name = "Space Explorer", Description = "Explore the galaxy" };
            var game2 = new Game { Name = "Dungeon Crawl", Description = "Classic dungeon adventure" };
            context.Games.AddRange(game1, game2);
            context.SaveChanges();

            // Achievements
            var ach1 = new Achievement { GameId = game1.Id, Name = "First Launch", Description = "Complete tutorial" };
            var ach2 = new Achievement { GameId = game1.Id, Name = "Orbit Master", Description = "Stable orbit" };
            var ach3 = new Achievement { GameId = game1.Id, Name = "Cosmic Pioneer", Description = "Visit 10 planets" };
            var ach4 = new Achievement { GameId = game2.Id, Name = "Treasure Hunter", Description = "Open 50 chests" };
            var ach5 = new Achievement { GameId = game2.Id, Name = "Dragon Slayer", Description = "Defeat final boss" };
            context.Achievements.AddRange(ach1, ach2, ach3, ach4, ach5);
            context.SaveChanges();

            // Highscores
            context.Highscores.AddRange(
                new Highscore { UserId = player1.Id, GameId = game1.Id, Score = 1200 },
                new Highscore { UserId = player2.Id, GameId = game1.Id, Score = 980 },
                new Highscore { UserId = badplayer.Id, GameId = game1.Id, Score = 1500 },
                new Highscore { UserId = player1.Id, GameId = game2.Id, Score = 450 },
                new Highscore { UserId = player2.Id, GameId = game2.Id, Score = 720 }
            );
            context.SaveChanges();

            // User achievements
            context.UserAchievements.AddRange(
                new UserAchievement { UserId = player1.Id, AchievementId = ach1.Id },
                new UserAchievement { UserId = player1.Id, AchievementId = ach2.Id },
                new UserAchievement { UserId = player2.Id, AchievementId = ach4.Id }
            );
            context.SaveChanges();

            // Report from supervisor
            context.Reports.Add(new Report
            {
                ReporterId = supervisor.Id,
                ReportedUserId = badplayer.Id,
                GameId = game1.Id,
                Reason = "Suspicious high score - possible cheating"
            });
            context.SaveChanges();
        }
    }
}