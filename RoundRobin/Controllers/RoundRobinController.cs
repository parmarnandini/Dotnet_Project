using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RoundRobin.Models;


namespace RoundRobin.Controllers
{
    public class RoundRobinController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Calculate(List<Process> processes, int timeQuantum)
        {
            var executedProcesses = new List<Process>();
            var queue = new Queue<Process>(processes.OrderBy(p => p.ArrivalTime));
            var currentTime = 0;

            while (queue.Count > 0)
            {
                var currentProcess = queue.Dequeue();
                var executionTime = Math.Min(timeQuantum, currentProcess.BurstTime);

                currentTime += executionTime;
                currentProcess.BurstTime -= executionTime;

                if (currentProcess.BurstTime > 0)
                {
                    queue.Enqueue(currentProcess);
                }
                else
                {
                    currentProcess.TurnaroundTime = currentTime - currentProcess.ArrivalTime;
                    executedProcesses.Add(currentProcess);
                }
            }

            ViewBag.GanttChart = GenerateGanttChart(executedProcesses);
            return View("~/Views/Home/Result.cshtml", executedProcesses);
        }

        private string GenerateGanttChart(List<Process> processes)
        {
            var ganttChart = new StringBuilder();
            var currentTime = 0;

            ganttChart.AppendLine("<table border='1'><tr><th>Time</th>");

            foreach (var process in processes.OrderBy(p => p.TurnaroundTime))
            {
                ganttChart.Append($"<th>P{process.Id}</th>");
            }

            ganttChart.AppendLine("</tr>");

            foreach (var process in processes.OrderBy(p => p.TurnaroundTime))
            {
                ganttChart.Append($"<tr><td>{currentTime}</td>");

                for (var i = 0; i < process.TurnaroundTime; i++)
                {
                    ganttChart.Append($"<td>{process.Id}</td>");
                }

                ganttChart.AppendLine("</tr>");
                currentTime += process.TurnaroundTime;
            }

            ganttChart.AppendLine("</table>");
            return ganttChart.ToString();
        }
    }
}
