using Microsoft.EntityFrameworkCore;
namespace Scheduling_Simulator.Models
{

    public class AppDbContext : DbContext
    {
        public DbSet<User> User { get; set; }
        public DbSet <Simulation> Simulations { get; set; }

        public DbSet<SimulationInput> SimulationInputs { get; set; }
        public DbSet<SimulationOutput> SimulationOutput { get; set; }

        public DbSet<OSQuiz> Quiz { get; set; }

        public DbSet<Result> Results { get; set; }



        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
    }
}
