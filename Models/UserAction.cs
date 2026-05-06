using System;

namespace DashboardApp.Models
{
    public class UserAction
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string ActionType { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int? AdminId { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;

        public virtual User? User { get; set; }
        public virtual User? Admin { get; set; }
    }
}