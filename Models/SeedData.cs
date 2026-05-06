using System;
using System.Linq;
using DashboardApp.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace DashboardApp.Data
{
    public static class SeedData
    {
        public static void Initialize(AppDbContext context)
        {
            if (context.Users.Any()) return;   // already seeded

            // Create users
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
            var player = new User
            {
                Username = "player1",
                PasswordHash = HashPassword("player123"),
                PermissionLevel = "player",
                CreatedAt = DateTime.Now
            };
            var bannedPlayer = new User
            {
                Username = "badplayer",
                PasswordHash = HashPassword("bad123"),
                PermissionLevel = "player",
                Banned = true,
                BanEnd = DateTime.Now.AddDays(7),
                CreatedAt = DateTime.Now
            };
            context.Users.AddRange(admin, supervisor, player, bannedPlayer);
            context.SaveChanges();   // to get Ids

            // Actions against accounts (only admin can see)
            context.UserActions.Add(new UserAction
            {
                UserId = bannedPlayer.Id,
                ActionType = "BAN",
                Description = "Cheating",
                AdminId = admin.Id,
                Timestamp = DateTime.Now
            });

            // Games
            var game1 = new Game { Name = "Space Explorer", Description = "A space exploration game" };
            var game2 = new Game { Name = "Dungeon Crawl", Description = "A classic dungeon crawler" };
            context.Games.AddRange(game1, game2);
            context.SaveChanges();

            // Achievements for game1 (Space Explorer) – 3 achievements
            var ach1 = new Achievement { GameId = game1.Id, Name = "First Launch", Description = "Complete the tutorial" };
            var ach2 = new Achievement { GameId = game1.Id, Name = "Orbit Master", Description = "Achieve a stable orbit" };
            var ach3 = new Achievement { GameId = game1.Id, Name = "Cosmic Pioneer", Description = "Visit 10 planets" };
            // Achievements for game2 (Dungeon Crawl) – 2 achievements
            var ach4 = new Achievement { GameId = game2.Id, Name = "Treasure Hunter", Description = "Open 50 chests" };
            var ach5 = new Achievement { GameId = game2.Id, Name = "Dragon Slayer", Description = "Defeat the final boss" };
            context.Achievements.AddRange(ach1, ach2, ach3, ach4, ach5);
            context.SaveChanges();

            // Some game sessions (for statistics)
            context.GameSessions.AddRange(
                new GameSession { UserId = player.Id, GameId = game1.Id, StartTime = DateTime.Now.AddDays(-3), EndTime = DateTime.Now.AddDays(-3).AddHours(2) },
                new GameSession { UserId = player.Id, GameId = game1.Id, StartTime = DateTime.Now.AddDays(-2), EndTime = DateTime.Now.AddDays(-2).AddHours(1) },
                new GameSession { UserId = admin.Id, GameId = game2.Id, StartTime = DateTime.Now.AddDays(-1), EndTime = DateTime.Now.AddDays(-1).AddMinutes(45) },
                new GameSession { UserId = supervisor.Id, GameId = game2.Id, StartTime = DateTime.Now.AddHours(-3), EndTime = null }   // still playing
            );
            context.SaveChanges();
        }

        private static string HashPassword(string password)
        {
            byte[] salt = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08 }; // fixed salt for demo (not secure)
            return Convert.ToBase64String(
                KeyDerivation.Pbkdf2(password, salt, KeyDerivationPrf.HMACSHA256, 10000, 256 / 8));
        }
    }
}