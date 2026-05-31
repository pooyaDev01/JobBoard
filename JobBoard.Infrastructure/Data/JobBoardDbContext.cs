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
        public DbSet<Company> Companies {  get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Company>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(x => x.Description)
                    .IsRequired()
                    .HasMaxLength(2000);

                entity.Property(x => x.Website)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(x => x.Location)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<Job>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Title)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(x => x.Description)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.Property(x => x.Location)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(x => x.SalaryMin)
                    .HasColumnType("decimal(18,2)");

                entity.Property(x => x.SalaryMax)
                    .HasColumnType("decimal(18,2)");

                entity.HasOne(x => x.Company)
                    .WithMany(c => c.Jobs)
                    .HasForeignKey(x => x.CompanyId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
