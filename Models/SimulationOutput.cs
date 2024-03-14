using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace Scheduling_Simulator.Models
{
    [PrimaryKey(nameof(S_Id), nameof(PId))]

    public class SimulationOutput
    {
 
        public virtual int S_Id { get; set; }
        [ForeignKey("S_Id")]
        public virtual Simulation? simulation { get; set; }
     
        public int PId { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Completion Time must be a non-negative number")]
        public int CompletionTime { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Turn Around Time must be a non-negative number")]
        public int TurnAroundTime { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Wait Time must be a non-negative number")]
        public int WaitTime { get; set; }
        public bool ByUser { get; set; }
        
    }
}
