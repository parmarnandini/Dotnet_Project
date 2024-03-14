using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;

namespace Scheduling_Simulator.Models
{
    [PrimaryKey(nameof(S_Id), nameof(PId))]

    public class SimulationInput
    {
  
        public virtual int S_Id { get; set; }
        [ForeignKey("S_Id")]
        public virtual Simulation? simulation { get; set; }

        public int PId { get; set; }

        [Required(ErrorMessage = "Arrival Time is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Arrival Time must be a non-negative number")]
        public int ArrivalTime { get; set; }

        [Required(ErrorMessage = "Burst Time is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Burst Time must be a positive number")]
        public int BurstTime { get; set; }
    }
}
