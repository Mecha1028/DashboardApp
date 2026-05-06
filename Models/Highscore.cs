namespace DashboardApp.Models
{
    public class Highscore
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int GameId { get; set; }
        public int Score { get; set; }             // higher = better

        public User? User { get; set; }
        public Game? Game { get; set; }
    }
}