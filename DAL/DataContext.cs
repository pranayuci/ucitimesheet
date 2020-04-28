using DAL.Data.DataSeed;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models.BO;
using Models.BO.ViewModels;

namespace DAL
{
    public class DataContext : IdentityDbContext<User>
    {
        private readonly IDataSeeder dataSeeder;

        public DbSet<Employee> Employees { get; set; }
        public DbSet<TimesheetEntry> TimesheetEntries { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<WeeksOfYear> WeeksOfYear { get; set; }

        public virtual DbQuery<TotalHoursConsultantWiseVm> DbPOProductSetupQuery { get; set; }


        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
            dataSeeder = new DataSeeder();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //dataSeeder.SeedData(modelBuilder);
        }
    }
}