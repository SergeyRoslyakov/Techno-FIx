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
    }
}
