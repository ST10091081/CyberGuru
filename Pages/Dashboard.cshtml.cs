using CyberGamify.Data;
using CyberGamify.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CyberGamify.Pages
{
    [Authorize]
    public class DashboardModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public DashboardModel(ApplicationDbContext db) => _db = db;

        public string Username { get; set; } = "";
        public int Points { get; set; }
        public int Level { get; set; }
        public int ProgressPercent { get; set; }
        public List<string> Badges { get; set; } = new();
        public List<GameTile> Games { get; set; } = new();
        public List<(string userId, int score)> TopScores { get; set; } = new();

        public async Task OnGet()
        {
            Username = User.Identity?.Name ?? "Player";
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";

            // Points = sum of all quiz attempt scores
            Points = await _db.QuizAttempts
                .Where(a => a.UserId == userId)
                .SumAsync(a => (int?)a.Score) ?? 0;

            Level = Points / 100 + 1;
            var currentLevelBase = (Level - 1) * 100;
            var nextLevelBase = Level * 100;
            ProgressPercent = nextLevelBase == currentLevelBase
                ? 0
                : (int)((Points - currentLevelBase) * 100.0 / (nextLevelBase - currentLevelBase));

            // Simple badges
            if (Points >= 100) Badges.Add("Beginner Defender");
            if (Points >= 300) Badges.Add("Threat Hunter");
            if (Points >= 600) Badges.Add("Cyber Guardian");

            // Ensure GameProgress row exists (optional tracking)
            var gp = await _db.GameProgresses.FirstOrDefaultAsync(g => g.UserId == userId);
            if (gp == null)
            {
                gp = new GameProgress
                {
                    UserId = userId,
                    Points = Points,
                    Badge = Badges.LastOrDefault() ?? "",
                    LastPlayed = DateTime.UtcNow
                };
                _db.GameProgresses.Add(gp);
                await _db.SaveChangesAsync();
            }
            else
            {
                gp.Points = Points;
                gp.Badge = Badges.LastOrDefault() ?? gp.Badge;
                gp.LastPlayed = DateTime.UtcNow;
                await _db.SaveChangesAsync();
            }

            Games = new List<GameTile>
            {
                new("Phishing Quiz","/Games/PhishingQuiz","Identify phishing attempts","???"),
                new("Password Game","/Games/PasswordGame","Strengthen password habits","??"),
                new("General Quiz","/Games/Quiz","Mixed cybersecurity questions","??")
            };

            TopScores = await _db.QuizAttempts
                .OrderByDescending(a => a.Score)
                .Take(5)
                .Select(a => new ValueTuple<string,int>(a.UserId, a.Score))
                .ToListAsync();
        }

        public record GameTile(string Title, string Url, string Description, string Emoji);
    }
}