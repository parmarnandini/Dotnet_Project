using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Scheduling_Simulator.Models
{
    [PrimaryKey(nameof(UserId), nameof(Quiz_Id))]
    public class Result
    {
        public virtual int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User? user { get; set; }

        public virtual int Quiz_Id { get; set; }
        [ForeignKey("Quiz_Id")]
        public virtual OSQuiz? quiz { get; set; }

        public int Score {  get; set; }

    }
}
