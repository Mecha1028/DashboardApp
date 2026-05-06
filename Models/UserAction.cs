using System;

namespace DashboardApp.Models
{
    public class UserAction
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string ActionType { get; set; } = null!;  // e.g. "BAN", "UNBAN"
        public string Description { get; set; } = null!;
        public int AdminId { get; set; }                 // admin who performed action
        public DateTime Timestamp { get; set; } = DateTime.Now;

        public User? User { get; set; }
        public User? Admin { get; set; }
    }
}