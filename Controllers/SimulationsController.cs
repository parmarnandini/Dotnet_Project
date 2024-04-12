using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scheduling_Simulator.Models;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Scheduling_Simulator.Controllers
{
    public class SimulationController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;
        public SimulationController(AppDbContext context, UserManager<User> userManager) 
        { 
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View("Index");
        }

        [HttpPost]
        public IActionResult SubmitIndex(int numProcesses, string algorithm)
        {
            return RedirectToAction("Input", new { numProcesses = numProcesses, algorithm = algorithm });

        }

            public async Task<IActionResult> Input(int numProcesses, string algorithm)
            {






            var username = User.Identity.Name;

            // Find the user by email
            var user = await _userManager.FindByEmailAsync(username);
            int userId;

            if (user != null)
            {
                userId = user.UserId; // Assuming UserId is the integer property representing the user ID
                                          // Now you have the userId as an integer
            }
            else
            {
                // Handle the case where the user is not found
                userId = 0;
            }

            var simulationData = new Simulation
            {
                Algorithm = algorithm,
                NumProcesses = numProcesses,
                UserId=userId
            };

           

            _context.Simulation.Add(simulationData);
            _context.SaveChanges();





            ViewBag.NumProcesses = numProcesses;
            ViewBag.Algorithm = algorithm;
            ViewBag.Processes = Enumerable.Range(0, numProcesses).Select(i => (char)('A' + i)).ToList(); 
            return View();
        }

        [HttpPost]
        public IActionResult Calculate(List<SimulationInput> inputs, string algorithm, string selectedOption,int quantumTime)
        {

            //storing data
            //TempData["inputList"] = inputs;
            TempData["quantum"] = quantumTime;
            

            List<SimulationOutput> outputs = new List<SimulationOutput>();

            Console.WriteLine("selectedOP" + selectedOption);


            if (algorithm == "FCFS")
            {
                outputs = CalculateFCFS(inputs);
            }
            else if (algorithm == "RR")
            {
                
                outputs = CalculateRoundRobin(inputs, quantumTime);
            }

            ViewBag.Inputs = inputs;
            ViewBag.Outputs = outputs;
            ViewBag.NumProcesses = inputs.Count;
            ViewBag.Algorithm = algorithm;

            if(selectedOption == "userTry")
            {
                TempData["userAns"] = true;
                return View("UserInput");
            }
            else
            {
                TempData["userAns"] = false;
                return View("Result");
            }

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
                    PId = inputs[i].PId , // Assuming PId starts from 1
                    CompletionTime = CT[i],
                    TurnAroundTime = TAT[i],
                    WaitTime = WT[i],
                    ByUser = false
                });
            }

            outputs = outputs.OrderBy(i => i.PId).ToList();

            //TempData["outputList"] = outputs;
            return outputs;
        }

        /*
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
        */



        private List<SimulationOutput> CalculateRoundRobin(List<SimulationInput> inputs, int quantumTime)
        {
            List<SimulationOutput> outputs = new List<SimulationOutput>();

            int n = inputs.Count;
            inputs = inputs.OrderBy(i => i.ArrivalTime).ToList();
            int[] remainingBurstTimes = inputs.Select(input => input.BurstTime).ToArray();
            int[] completionTimes = new int[n];
            int[] turnaroundTimes = new int[n];
            int[] waitingTimes = new int[n];
            bool flag=true;

            Queue<int> readyQueue = new Queue<int>();
            int currentTime = 0;
            int arrivedIndex = 0;
            int currentIndex=-1;

            while ((readyQueue.Count > 0 || arrivedIndex < n) || flag)
            {
                // Add processes that have arrived and not yet executed to the ready queue


                while (arrivedIndex < n && inputs[arrivedIndex].ArrivalTime <= currentTime)
                {
                    readyQueue.Enqueue(arrivedIndex);
                    arrivedIndex++;
                }
                Console.WriteLine();

                if (currentIndex != -1 && remainingBurstTimes[currentIndex] > 0)
                {
                    readyQueue.Enqueue(currentIndex);
                }

                

                if (readyQueue.Count == 0)
                {
                    currentTime = inputs[arrivedIndex].ArrivalTime;
                    continue;
                }

                Console.WriteLine("count in ready queue " + readyQueue.Count);
                currentIndex = readyQueue.Dequeue();
                Console.WriteLine("count in ready queue " + readyQueue.Count);

                int burstTime = Math.Min(quantumTime, remainingBurstTimes[currentIndex]);

                // Execute the process for the burst time
                currentTime += burstTime;
                remainingBurstTimes[currentIndex] -= burstTime;


                Console.Write("PID"+ inputs[currentIndex].PId);
                Console.WriteLine("  RTime" + remainingBurstTimes[currentIndex]);
                // If the process is not yet completed, add it back to the ready queue
                if (remainingBurstTimes[currentIndex] <= 0)
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



                flag = false;
                for (int i = 0; i < n; i++)
                {
                    if (remainingBurstTimes[i] >0) 
                    {
                        flag = true;
                        continue;
                    }
                }
                
                
                

            }


            /* Console.WriteLine("Current time: " + currentTime);
             Console.WriteLine("Arrived index: " + arrivedIndex);
             Console.WriteLine("Current index: " + currentIndex);
             Console.WriteLine("Remain time: "+ remainingBurstTimes[currentIndex]);
             Console.WriteLine("n: " + n);
             Console.WriteLine("count in ready queue " + readyQueue.Count);
             */


            // Group the output by process ID and calculate the total turnaround and waiting times
            /*var groupedOutputs = outputs.GroupBy(o => o.PId)
                                        .Select(g => new SimulationOutput
                                        {
                                            PId = g.Key,
                                            CompletionTime = g.Max(o => o.CompletionTime),
                                            TurnAroundTime = g.Sum(o => o.TurnAroundTime),
                                            WaitTime = g.Sum(o => o.WaitTime)
                                        })
                                        .OrderBy(o => o.PId)
                                        .ToList();

            return groupedOutputs;*/

            outputs = outputs.OrderBy(i => i.PId).ToList();
           // TempData["outputList"] = outputs;
            return outputs;
        }


        public IActionResult checkAnswer(List<SimulationOutput> answers, string algorithm, List<SimulationInput> inputs)
        {
            int n = answers.Count;
            List<bool[]> result = new List<bool[]>();
            List<SimulationOutput> outputs = new List<SimulationOutput>();

            bool[] CT = new bool[n];
            bool[] TAT= new bool[n];
            bool[] WT = new bool[n];

           

            if (algorithm == "FCFS")
            {
                outputs = CalculateFCFS(inputs);
            }
            else if (algorithm == "RR")
            {
                int quantumTime = 1;
                outputs = CalculateRoundRobin(inputs, quantumTime);
            }


                for (int i = 0; i < n; i++)
            {

                if (outputs[i].CompletionTime == answers[i].CompletionTime)
                {
                    CT[i] = true;
                }
                else
                {
                    CT[i] = false;
                }

                if (outputs[i].TurnAroundTime == answers[i].TurnAroundTime)
                {
                    TAT[i] = true;
                }
                else
                {
                    TAT[i] = false;
                }

                if (outputs[i].WaitTime == answers[i].WaitTime)
                {
                    WT[i] = true;
                }
                else
                {
                    WT[i] = false;
                }
                
            }

            result.Add(CT);
            result.Add(TAT);
            result.Add(WT);

            ViewBag.Answer=answers;
            ViewBag.Result=result;
            ViewBag.selectedOption = "userTry";

           // TempData["userAnsList"] = answers;
            return View("Result");
        }




       
        

    }
}
