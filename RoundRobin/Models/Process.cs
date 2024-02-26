namespace RoundRobin.Models
{
    public class Process
    {
        public int Id { get; set; }
        public int ArrivalTime { get; set; }
        public int BurstTime { get; set; }
        public int TurnaroundTime { get; set; }
    }
}
