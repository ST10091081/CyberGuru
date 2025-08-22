using System;

namespace CyberGamify.Models
{
    public class QuizAttempt
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Game { get; set; }
        public int Score { get; set; }
        public int TotalQuestions { get; set; }
        public int DurationSeconds { get; set; }
        public DateTime CreatedUtc { get; set; }
    }
}

