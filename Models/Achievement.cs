namespace DashboardApp.Models
{
    public class Achievement
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;

        public virtual Game? Game { get; set; }
    }
}