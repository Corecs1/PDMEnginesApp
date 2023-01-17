using Microsoft.EntityFrameworkCore;
using PDMEnginesApp.model.entity;

namespace PDMEnginesApp.config
{
    public class DataBaseConfig : DbContext
    {
        public DbSet<Engine> engines { get; set; } = null!;
        public DbSet<EngineComponent> components { get; set; } = null!;
        public DbSet<ComponentComponentAmount> componentComponentAmounts { get; set; } = null!;
        public DbSet<EngineComponentAmount> engineComponentAmounts { get; set; } = null!;

        public DataBaseConfig()
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=PDMEngines;Trusted_Connection=True; MultipleActiveResultSets=True;");
        }

    }
}
