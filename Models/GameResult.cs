using System;

namespace CyberGamify.Models
{
    // Record a single game play result. Useful for leaderboards and analytics.
    public class GameResult
    {
        public int Id { get; set; }
        public string UserId { get; set; }           // FK to ApplicationUser.Id
        public string GameType { get; set; }         // E.g. "Phishing", "Password", "Quiz"
        public int Score { get; set; }
        public DateTime PlayedAt { get; set; } = DateTime.UtcNow;

        // Navigation property (optional)
        public ApplicationUser User { get; set; }
    }
}
