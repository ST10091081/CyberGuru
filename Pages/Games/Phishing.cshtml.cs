using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CyberGamify.Data;
using CyberGamify.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace CyberGamify.Pages.Games
{
    [Authorize] // user must be logged in to play and save score
    public class PhishingModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        public PhishingModel(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public void OnGet()
        {
            // Nothing needed; all game logic is client-side and will post results
        }

        // Handler for AJAX POST: receives { score: int }
        // NOTE: For hackathon speed we disable anti-forgery here; in production, include antiforgery token.
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> OnPostSubmitScoreAsync([FromBody] SubmitScoreDto dto)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            // Save the game result
            var result = new GameResult
            {
                UserId = user.Id,
                GameType = "Phishing",
                Score = dto.Score
            };
            _db.GameResults.Add(result);

            // Update user's XP
            user.XP += dto.Score;

            await _db.SaveChangesAsync();

            return new JsonResult(new { success = true, newXP = user.XP });
        }

        // DTO to bind the incoming JSON
        public class SubmitScoreDto
        {
            public int Score { get; set; }
        }
    }
}
