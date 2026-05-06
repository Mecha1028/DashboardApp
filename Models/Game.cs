using System.Collections.Generic;

namespace DashboardApp.Models
{
    public class Game
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;

        public ICollection<Achievement> Achievements { get; set; } = new HashSet<Achievement>();
        public ICollection<GameSession> GameSessions { get; set; } = new HashSet<GameSession>();
    }
}