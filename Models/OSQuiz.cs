using System.ComponentModel.DataAnnotations;

namespace Scheduling_Simulator.Models
{
    public class OSQuiz
    {
        [Key]
        public int Quiz_Id { get; set; }
        [Required]
        public string Question { get; set; }
        [Required]
        public string Choice1 { get; set; }
        [Required]
        public string Choice2 { get; set; }
        [Required]
        public string Choice3 { get; set; }
        public string CorrectAnswer { get; set; }

      

    }
}
