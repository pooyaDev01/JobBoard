using JobBoard.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Infrastructure.Data
{
    public class JobBoardDbContext : DbContext
    {
        public JobBoardDbContext(DbContextOptions<JobBoardDbContext> options) : base(options)
        {
            
        }
        public DbSet<Job> Jobs { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Job>(entity =>
            {
                entity.Property(j => j.SalaryMin)
                    .HasPrecision(18, 2);

                entity.Property(j => j.SalaryMax)
                    .HasPrecision(18, 2);
            });
        }
    }
}
