using CyberGamify.Data;
using CyberGamify.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CyberGamify.Pages
{
    [Authorize]
    public class LeaderboardModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public LeaderboardModel(ApplicationDbContext db) => _db = db;

        public List<QuizAttempt> Top { get; set; } = new();

        public async Task OnGet()
        {
            Top = await _db.QuizAttempts
                .OrderByDescending(a => a.Score)
                .ThenBy(a => a.DurationSeconds)
                .Take(10)
                .ToListAsync();
        }
    }
}

