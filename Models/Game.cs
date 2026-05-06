using System.Collections.Generic;

namespace DashboardApp.Models
{
    public class Game
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;

        public ICollection<Achievement> Achievements { get; set; } = new HashSet<Achievement>();
        public ICollection<Highscore> Highscores { get; set; } = new HashSet<Highscore>();
        public ICollection<Report> Reports { get; set; } = new HashSet<Report>();
    }
}