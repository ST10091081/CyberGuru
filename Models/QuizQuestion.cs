namespace CyberGamify.Models
{
    public class QuizQuestion
    {
        public int Id { get; set; }
        public string Category { get; set; }
        public string Text { get; set; }
        public string OptionA { get; set; }
        public string OptionB { get; set; }
        public string OptionC { get; set; }
        public string OptionD { get; set; }
        // 0..3 index of correct option
        public int CorrectIndex { get; set; }
        public int Difficulty { get; set; }
    }
}
