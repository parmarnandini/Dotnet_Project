using Microsoft.EntityFrameworkCore;
namespace Scheduling_Simulator.Models
{

    public class AppDbContext : DbContext
    {
        public DbSet<User> User { get; set; }
        public DbSet<Simulation> Simulation { get; set; }

        public DbSet<SimulationInput> SimulationInput { get; set; }
        public DbSet<SimulationOutput> SimulationOutput { get; set; }

        public DbSet<OSQuiz> OSQuiz { get; set; }

        public DbSet<Result> Result { get; set; }



        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
    }
}
