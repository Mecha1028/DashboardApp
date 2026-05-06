using System;

namespace DashboardApp.Models
{
    public class UserAchievement
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int AchievementId { get; set; }
        public DateTime EarnedAt { get; set; } = DateTime.Now;

        public User? User { get; set; }
        public Achievement? Achievement { get; set; }
    }
}