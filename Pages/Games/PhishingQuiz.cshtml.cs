using System.Linq;
using System.Security.Claims;
using CyberGamify.Data;
using CyberGamify.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CyberGamify.Pages.Games
{
    [Authorize]
    public class PhishingQuizModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public PhishingQuizModel(ApplicationDbContext db) => _db = db;

        public List<ClientQuestion> Questions { get; set; } = new();
        public int Level { get; private set; } = 1;

        public async Task OnGet(int level = 1)
        {
            Level = Math.Clamp(level, 1, 3);

            var qs = await _db.QuizQuestions
                .Where(q => q.Category == "Phishing" && q.Difficulty <= Level)
                .OrderBy(_ => Guid.NewGuid())
                .Take(10)
                .ToListAsync();

            Questions = qs.Select(q => new ClientQuestion
            {
                Id = q.Id,
                Text = q.Text,
                Options = new[] { q.OptionA, q.OptionB, q.OptionC, q.OptionD }
                    .Where(o => !string.IsNullOrWhiteSpace(o))
                    .ToArray()
            }).ToList();
        }

        public class SubmitDto
        {
            public int[] QuestionIds { get; set; } = Array.Empty<int>();
            public int[] SelectedIndices { get; set; } = Array.Empty<int>();
            public int DurationSeconds { get; set; }
        }

        public class ResultDto
        {
            public int Score { get; set; }
            public int Correct { get; set; }
            public int Wrong { get; set; }
            public int Total { get; set; }
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostSubmitAsync([FromBody] SubmitDto dto)
        {
            if (dto.QuestionIds.Length == 0 || dto.QuestionIds.Length != dto.SelectedIndices.Length)
                return BadRequest(new { error = "Invalid payload" });

            var questions = await _db.QuizQuestions
                .Where(q => dto.QuestionIds.Contains(q.Id))
                .ToListAsync();

            var map = questions.ToDictionary(q => q.Id, q => q.CorrectIndex);

            int correct = 0;
            for (int i = 0; i < dto.QuestionIds.Length; i++)
            {
                if (map.TryGetValue(dto.QuestionIds[i], out var ans) && dto.SelectedIndices[i] == ans)
                    correct++;
            }

            int total = dto.QuestionIds.Length;
            int wrong = total - correct;
            int score = Math.Max(0, correct * 20 - wrong * 5);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "anonymous";
            _db.QuizAttempts.Add(new QuizAttempt
            {
                UserId = userId,
                Score = score,
                TotalQuestions = total,
                DurationSeconds = Math.Max(0, dto.DurationSeconds),
                CreatedUtc = DateTime.UtcNow
            });
            await _db.SaveChangesAsync();

            return new JsonResult(new ResultDto { Score = score, Correct = correct, Wrong = wrong, Total = total });
        }

        public class ClientQuestion
        {
            public int Id { get; set; }
            public string Text { get; set; } = string.Empty;
            public string[] Options { get; set; } = Array.Empty<string>();
        }
    }
}