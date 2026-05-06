using System;

namespace DashboardApp.Models
{
    public class Report
    {
        public int Id { get; set; }
        public int ReporterId { get; set; }        // supervisor who reported
        public int ReportedUserId { get; set; }
        public int GameId { get; set; }
        public string Reason { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public User? Reporter { get; set; }
        public User? ReportedUser { get; set; }
        public Game? Game { get; set; }
    }
}