using System;

namespace DashboardApp.Models
{
    public class UserAchievement
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int AchievementId { get; set; }
        public DateTime EarnedAt { get; set; } = DateTime.Now;

        public virtual User? User { get; set; }
        public virtual Achievement? Achievement { get; set; }
    }
}