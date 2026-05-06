using System;

namespace DashboardApp.Models
{
    public class GameSession
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int GameId { get; set; }
        public DateTime StartTime { get; set; } = DateTime.Now;
        public DateTime? EndTime { get; set; }

        public virtual User? User { get; set; }
        public virtual Game? Game { get; set; }
    }
}