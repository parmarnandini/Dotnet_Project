namespace Scheduling_Simulator.Models
{
    public class Process
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Algorithm { get; set; }
        public int NumProcesses { get; set; }
        public int ArrivalTime { get; set; }
        public int BurstTime { get; set; }
        public int? QuantumTime { get; set; } // optional 
        public bool IsAnswered { get; set; }
        public int UserCalculatedAnswer { get; set; }
        public int TurnAroundTime { get; set; }
        public int WaitTime { get; set; }
    }
}
