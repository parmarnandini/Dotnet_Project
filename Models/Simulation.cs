using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
namespace Scheduling_Simulator.Models
{
    public class Simulation
    {
        [Key]
        public int S_Id { get; set; }

        public virtual int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User? user { get; set; }

        [Required(ErrorMessage = "Algorithm is required")]
         public string Algorithm { get; set; }

        [Required(ErrorMessage = "Number of Processes is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Number of Processes must be a positive number")]
        public int NumProcesses { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Quantum Time must be a positive number")]
        public int? QuantumTime { get; set; } // optional 

        public bool IsAnswered { get; set; }

        [StringLength(500, ErrorMessage = "Simulation Note must not exceed 500 characters")]
        public string S_Note { get; set; }
    }
}
