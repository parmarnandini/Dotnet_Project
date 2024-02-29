namespace Scheduling_Simulator.Models
{
    public class Quiz
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Question { get; set; }
        public string CorrectAnswer { get; set; }
        public string UserAnswer { get; set; }
        public bool IsCorrect { get; set; }
    }
}
