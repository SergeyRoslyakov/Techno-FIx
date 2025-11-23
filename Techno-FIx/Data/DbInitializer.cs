using Techno_FIx.Data;
using Techno_FIx.Models;

namespace Techno_Fix.Data
{
    public static class DbInitializer
    {
        public static void Initialize(AppDbContext context)
        {
            context.Database.EnsureCreated();
            if (context.Users.Any())
            {
                return; 
            }

            var technicians = new Technician[]
            {
        new Technician { FirstName = "Алексей", LastName = "Смирнов", Specialization = "Ноутбуки и ПК", Phone = "+79991112233" },
        new Technician { FirstName = "Ольга", LastName = "Кузнецова", Specialization = "Смартфоны и планшеты", Phone = "+79994445566" }
            };
            context.Technicians.AddRange(technicians);
            context.SaveChanges();
            var users = new User[]
            {
        new User { Username = "admin", Email = "admin@technofix.ru", Password = "admin123", Role = "Admin", CreatedAt = DateTime.UtcNow },
        new User { Username = "tech1", Email = "alexey@technofix.ru", Password = "tech123", Role = "Technician", TechnicianId = 1, CreatedAt = DateTime.UtcNow },
        new User { Username = "user1", Email = "client@mail.com", Password = "user123", Role = "User", CreatedAt = DateTime.UtcNow }
            };
            context.Users.AddRange(users);

            var clients = new Client[]
            {
        new Client { FirstName = "Иван", LastName = "Петров", Phone = "+79991234567", Email = "ivan@mail.com" },
        new Client { FirstName = "Мария", LastName = "Сидорова", Phone = "+79997654321", Email = "maria@mail.com" }
            };
            context.Clients.AddRange(clients);

            var services = new Service[]
            {
        new Service { Name = "Диагностика", Description = "Полная диагностика устройства", Price = 500.00m },
        new Service { Name = "Замена экрана", Description = "Замена дисплея устройства", Price = 3000.00m },
        new Service { Name = "Чистка от пыли", Description = "Чистка системы охлаждения", Price = 1500.00m }
            };
            context.Services.AddRange(services);

            context.SaveChanges();
        }
    }
}