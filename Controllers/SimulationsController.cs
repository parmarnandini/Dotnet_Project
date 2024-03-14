using Microsoft.AspNetCore.Mvc;
using Scheduling_Simulator.Models;
using System.Collections.Generic;
using System.Linq;

namespace Scheduling_Simulator.Controllers
{
    public class SimulationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(int numProcesses, string algorithm)
        {
            return RedirectToAction("Input", new { numProcesses = numProcesses, algorithm = algorithm });
        }

        public IActionResult Input(int numProcesses, string algorithm)
        {
            ViewBag.NumProcesses = numProcesses;
            ViewBag.Algorithm = algorithm;
            ViewBag.Processes = Enumerable.Range(0, numProcesses).Select(i => (char)('A' + i)).ToList(); 
            return View();
        }

        [HttpPost]
        public IActionResult Calculate(List<SimulationInput> inputs, string algorithm)
        {
            List<SimulationOutput> outputs = new List<SimulationOutput>();

            if (algorithm == "FCFS")
            {
                outputs = CalculateFCFS(inputs);
            }
            else if (algorithm == "RR")
            {
                int quantumTime = 1; 
                outputs = CalculateRoundRobin(inputs, quantumTime);
            }

            ViewBag.Outputs = outputs;

            return View("Result");
        }

        private List<SimulationOutput> CalculateFCFS(List<SimulationInput> inputs)
        {
            List<SimulationOutput> outputs = new List<SimulationOutput>();

            // Sort the inputs based on arrival time
            inputs = inputs.OrderBy(i => i.ArrivalTime).ToList();

            int n = inputs.Count;
            int[] CT = new int[n]; // Completion times
            int[] TAT = new int[n]; // Turnaround times
            int[] WT = new int[n]; // Waiting times

            CT[0] = inputs[0].ArrivalTime + inputs[0].BurstTime; // Completion time for the first process
            TAT[0] = CT[0] - inputs[0].ArrivalTime; // Turnaround time for the first process
            WT[0] = TAT[0] - inputs[0].BurstTime; // Waiting time for the first process

            // Calculate completion times, turnaround times, and waiting times for each process
            for (int i = 1; i < n; i++)
            {
                CT[i] = Math.Max(CT[i - 1], inputs[i].ArrivalTime) + inputs[i].BurstTime; // Completion time
                TAT[i] = CT[i] - inputs[i].ArrivalTime; // Turnaround time
                WT[i] = TAT[i] - inputs[i].BurstTime; // Waiting time
            }

            // Create SimulationOutput objects for each process
            for (int i = 0; i < n; i++)
            {
                outputs.Add(new SimulationOutput
                {
                    PId = i , // Assuming PId starts from 1
                    CompletionTime = CT[i],
                    TurnAroundTime = TAT[i],
                    WaitTime = WT[i],
                    ByUser = false
                });
            }

            return outputs;
        }


        private List<SimulationOutput> CalculateRoundRobin(List<SimulationInput> inputs, int quantumTime)
        {
            List<SimulationOutput> outputs = new List<SimulationOutput>();

            int n = inputs.Count;
            int[] remainingBurstTimes = inputs.Select(input => input.BurstTime).ToArray();
            int[] completionTimes = new int[n];
            int[] turnaroundTimes = new int[n];
            int[] waitingTimes = new int[n];

            Queue<int> readyQueue = new Queue<int>();
            int currentTime = 0;
            int arrivedIndex = 0;

            while (readyQueue.Count > 0 || arrivedIndex < n)
            {
                // Add processes that have arrived and not yet executed to the ready queue
                while (arrivedIndex < n && inputs[arrivedIndex].ArrivalTime <= currentTime)
                {
                    readyQueue.Enqueue(arrivedIndex);
                    arrivedIndex++;
                }

                if (readyQueue.Count == 0)
                {
                    currentTime = inputs[arrivedIndex].ArrivalTime;
                    continue;
                }

                int currentIndex = readyQueue.Dequeue();
                int burstTime = Math.Min(quantumTime, remainingBurstTimes[currentIndex]);

                // Execute the process for the burst time
                currentTime += burstTime;
                remainingBurstTimes[currentIndex] -= burstTime;

                // If the process is not yet completed, add it back to the ready queue
                if (remainingBurstTimes[currentIndex] > 0)
                {
                    readyQueue.Enqueue(currentIndex);
                }
                else
                {
                    // Process completed
                    completionTimes[currentIndex] = currentTime; // Increment completion time by 1
                    turnaroundTimes[currentIndex] = completionTimes[currentIndex] - inputs[currentIndex].ArrivalTime;
                    waitingTimes[currentIndex] = turnaroundTimes[currentIndex] - inputs[currentIndex].BurstTime;

                    outputs.Add(new SimulationOutput
                    {
                        PId = inputs[currentIndex].PId,
                        CompletionTime = completionTimes[currentIndex],
                        TurnAroundTime = turnaroundTimes[currentIndex],
                        WaitTime = waitingTimes[currentIndex],
                        ByUser = false
                    });
                }

            }

            // Group the output by process ID and calculate the total turnaround and waiting times
            var groupedOutputs = outputs.GroupBy(o => o.PId)
                                        .Select(g => new SimulationOutput
                                        {
                                            PId = g.Key,
                                            CompletionTime = g.Max(o => o.CompletionTime),
                                            TurnAroundTime = g.Sum(o => o.TurnAroundTime),
                                            WaitTime = g.Sum(o => o.WaitTime)
                                        })
                                        .OrderBy(o => o.PId)
                                        .ToList();

            return groupedOutputs;
        }


    }
}
