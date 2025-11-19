using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Techno_FIx.Models;

namespace Techno_FIx.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Technician> Technicians { get; set; }
        public DbSet<RepairOrder> RepairOrders { get; set; }
        public DbSet<User> Users { get; set; } // Добавлено

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Существующие конфигурации...
            modelBuilder.Entity<Device>()
                .HasOne(d => d.Client)
                .WithMany(c => c.Devices)
                .HasForeignKey(d => d.ClientId);

            modelBuilder.Entity<RepairOrder>()
                .HasOne(ro => ro.Device)
                .WithMany(d => d.RepairOrders)
                .HasForeignKey(ro => ro.DeviceId);

            modelBuilder.Entity<RepairOrder>()
                .HasOne(ro => ro.Service)
                .WithMany(s => s.RepairOrders)
                .HasForeignKey(ro => ro.ServiceId);

            modelBuilder.Entity<RepairOrder>()
                .HasOne(ro => ro.Technician)
                .WithMany(t => t.RepairOrders)
                .HasForeignKey(ro => ro.TechnicianId);

            // Новая конфигурация для User
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasOne(u => u.Technician)
                .WithMany()
                .HasForeignKey(u => u.TechnicianId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}