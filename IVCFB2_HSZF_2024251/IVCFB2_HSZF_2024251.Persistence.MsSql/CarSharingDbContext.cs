using IVCFB2_HSZF_2024251.Model;
using Microsoft.EntityFrameworkCore;

namespace IVCFB2_HSZF_2024251.Persistence.MsSql
{
    public class CarSharingDbContext : DbContext
    {
        public CarSharingDbContext() { this.Database.EnsureCreated(); }

        public DbSet<Car> Cars { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Trip> Trips { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connStr = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=CarSharing;Integrated Security=True;MultipleActiveResultSets=true";
            optionsBuilder.UseSqlServer(connStr);
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
