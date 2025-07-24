using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PJ_P_Installation_Management_System.Models;
using System;

namespace PJ_P_Installation_Management_System.Data
{
    public class PJInstallationDbContext : DbContext
    {
        public PJInstallationDbContext(DbContextOptions<PJInstallationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<Staff> Staffs { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Installation> Installations { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Purchase relationships
            modelBuilder.Entity<Purchase>(entity =>
            {
                entity.HasOne(p => p.Product)
                    .WithMany(p => p.Purchases)
                    .HasForeignKey(p => p.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(p => p.Supplier)
                    .WithMany(s => s.Purchases)
                    .HasForeignKey(p => p.SupplierId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.Property(p => p.Status)
                    .HasMaxLength(50)
                    .IsRequired();
            });

            // Configure Product
            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(p => p.Name)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(p => p.Description)
                    .HasMaxLength(500);

                entity.Property(p => p.Price)
                    .HasColumnType("decimal(18,2)");

                entity.Property(p => p.StockQuantity)
                    .HasDefaultValue(0);
            });

            // Configure Supplier
            modelBuilder.Entity<Supplier>(entity =>
            {
                entity.Property(s => s.CompanyName)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(s => s.ContactPerson)
                    .HasMaxLength(50);

                entity.Property(s => s.Phone)
                    .HasMaxLength(20);

                entity.Property(s => s.Email)
                    .HasMaxLength(100);

                entity.Property(s => s.IsActive)
                    .HasDefaultValue(true);
            });

            // Add configurations for other entities as needed...
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Only used for design-time migration creation
                optionsBuilder.UseSqlServer("YourConnectionString");
            }

            optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
            optionsBuilder.EnableSensitiveDataLogging();
        }
    }
}