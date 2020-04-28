using Microsoft.EntityFrameworkCore;
using Models.BO;
using System;

namespace DAL.Data.DataSeed
{
    /// <inheritdoc/>
    public class DataSeeder : IDataSeeder
    {
        /// <inheritdoc/>
        public void SeedData(ModelBuilder modelBuilder)
        {
            //// Seeding Companies data
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = "845e5f1f-fd43-458d-83d9-44b2a8981eb3",
                    FirstName = "Pranay",
                    LastName = "Mohite",
                    UserName = "pranay@uci.com",
                    Email = "pranay@uci.com",
                    Client = "NYC",
                    Department = "Software",
                    ContractStartTime = DateTime.Now,
                    ContractEndTime = DateTime.Now.AddYears(1),
                    ProjectName = "Project 1"
                }         

           );
        }
    }
}
