using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DashboardApp.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required] public string Username { get; set; } = null!;
        [Required] public string PasswordHash { get; set; } = null!;
        [Required]
        [RegularExpression("player|supervisor|admin")]
        public string PermissionLevel { get; set; } = null!;
        public bool Banned { get; set; }
        public DateTime? BanEnd { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation
        public ICollection<UserAction> UserActions { get; set; } = new HashSet<UserAction>();
        public ICollection<Highscore> Highscores { get; set; } = new HashSet<Highscore>();
        public ICollection<UserAchievement> UserAchievements { get; set; } = new HashSet<UserAchievement>();
        public ICollection<Report> SubmittedReports { get; set; } = new HashSet<Report>();
    }
}