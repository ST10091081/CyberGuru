using CyberGamify.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CyberGamify.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<QuizQuestion> QuizQuestions => Set<QuizQuestion>();
        public DbSet<QuizAttempt> QuizAttempts => Set<QuizAttempt>();
        public DbSet<GameProgress> GameProgresses => Set<GameProgress>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Seed a few phishing questions (IDs must be fixed)
            builder.Entity<QuizQuestion>().HasData(
                new QuizQuestion
                {
                    Id = 1,
                    Category = "Phishing",
                    Text = "Which is a phishing red flag?",
                    OptionA = "Mismatched sender domain (e.g., paypa1.com)",
                    OptionB = "Email from your manager via company domain",
                    OptionC = "No links and purely informational",
                    OptionD = "Internal newsletter",
                    CorrectIndex = 0,
                    Difficulty = 1
                },
                new QuizQuestion
                {
                    Id = 2,
                    Category = "Phishing",
                    Text = "A message says your account will be closed in 2 hours unless you click a link. Best action?",
                    OptionA = "Click the link urgently",
                    OptionB = "Forward to friends",
                    OptionC = "Report to security and verify via official channel",
                    OptionD = "Reply with your password",
                    CorrectIndex = 2,
                    Difficulty = 1
                },
                new QuizQuestion
                {
                    Id = 3,
                    Category = "Phishing",
                    Text = "Hovering shows link text 'microsoft.com' but URL goes to 'microsofft-login.biz'. What is this?",
                    OptionA = "Tracking link",
                    OptionB = "Legitimate Microsoft page",
                    OptionC = "URL shortener",
                    OptionD = "Phishing via lookalike domain",
                    CorrectIndex = 3,
                    Difficulty = 2
                },
                new QuizQuestion
                {
                    Id = 4,
                    Category = "Phishing",
                    Text = "Attachment 'invoice.pdf.exe' is:",
                    OptionA = "A harmless PDF",
                    OptionB = "Likely malware disguised as a PDF",
                    OptionC = "A spreadsheet",
                    OptionD = "Encrypted archive",
                    CorrectIndex = 1,
                    Difficulty = 2
                },
                new QuizQuestion
                {
                    Id = 5,
                    Category = "Phishing",
                    Text = "Which is safest way to verify a bank alert email?",
                    OptionA = "Use the email's link",
                    OptionB = "Call the number in the email",
                    OptionC = "Open your bank app or type the bank URL manually",
                    OptionD = "Reply and ask if it's real",
                    CorrectIndex = 2,
                    Difficulty = 1
                }
            );
        }
    }
}
