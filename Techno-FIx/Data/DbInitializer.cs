using Techno_FIx.Data;
using Techno_FIx.Models;

namespace Techno_Fix.Data
{
    public static class DbInitializer
    {
        public static void Initialize(AppDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Clients.Any()) return;

            var clients = new Client[]
            {
                new Client { FirstName = "Иван", LastName = "Петров", Phone = "+79161234567", Email = "ivan@mail.ru" },
                new Client { FirstName = "Мария", LastName = "Сидорова", Phone = "+79161234568", Email = "maria@mail.ru" },
                new Client { FirstName = "Алексей", LastName = "Козлов", Phone = "+79161234569", Email = "alex@mail.ru" },
                new Client { FirstName = "Елена", LastName = "Васильева", Phone = "+79161234570", Email = "elena@mail.ru" },
                new Client { FirstName = "Дмитрий", LastName = "Николаев", Phone = "+79161234571", Email = "dmitry@mail.ru" }
            };
            context.Clients.AddRange(clients);
            context.SaveChanges();

            var devices = new Device[]
            {
                new Device { Type = "Ноутбук", Brand = "Lenovo", Model = "ThinkPad X1", SerialNumber = "SN001", ProblemDescription = "Не включается", ClientId = 1 },
                new Device { Type = "Смартфон", Brand = "Samsung", Model = "Galaxy S21", SerialNumber = "SN002", ProblemDescription = "Разбит экран", ClientId = 2 },
                new Device { Type = "ПК", Brand = "HP", Model = "Pavilion", SerialNumber = "SN003", ProblemDescription = "Не загружается ОС", ClientId = 3 },
                new Device { Type = "Планшет", Brand = "Apple", Model = "iPad Air", SerialNumber = "SN004", ProblemDescription = "Не работает тачскрин", ClientId = 4 },
                new Device { Type = "Ноутбук", Brand = "Asus", Model = "ZenBook", SerialNumber = "SN005", ProblemDescription = "Перегревается", ClientId = 5 }
            };
            context.Devices.AddRange(devices);
            context.SaveChanges();

            var services = new Service[]
            {
                new Service { Name = "Диагностика", Description = "Полная диагностика устройства", Price = 500.00m },
                new Service { Name = "Замена экрана", Description = "Замена дисплейного модуля", Price = 3000.00m },
                new Service { Name = "Чистка от пыли", Description = "Чистка системы охлаждения", Price = 1500.00m },
                new Service { Name = "Установка ОС", Description = "Установка операционной системы", Price = 2000.00m },
                new Service { Name = "Ремонт материнской платы", Description = "Диагностика и ремонт МП", Price = 5000.00m }
            };
            context.Services.AddRange(services);
            context.SaveChanges();

            var technicians = new Technician[]
            {
                new Technician { FirstName = "Андрей", LastName = "Смирнов", Specialization = "Ноутбуки", Phone = "+79161234572" },
                new Technician { FirstName = "Ольга", LastName = "Попова", Specialization = "Смартфоны", Phone = "+79161234573" },
                new Technician { FirstName = "Сергей", LastName = "Федоров", Specialization = "Стационарные ПК", Phone = "+79161234574" },
                new Technician { FirstName = "Наталья", LastName = "Морозова", Specialization = "Планшеты", Phone = "+79161234575" },
                new Technician { FirstName = "Артем", LastName = "Волков", Specialization = "Все виды техники", Phone = "+79161234576" }
            };
            context.Technicians.AddRange(technicians);
            context.SaveChanges();

            var repairOrders = new RepairOrder[]
            {
                new RepairOrder { DeviceId = 1, ServiceId = 1, TechnicianId = 1, TotalCost = 500.00m, Status = "Completed" },
                new RepairOrder { DeviceId = 2, ServiceId = 2, TechnicianId = 2, TotalCost = 3000.00m, Status = "InProgress" },
                new RepairOrder { DeviceId = 3, ServiceId = 3, TechnicianId = 3, TotalCost = 1500.00m, Status = "Ready" },
                new RepairOrder { DeviceId = 4, ServiceId = 4, TechnicianId = 4, TotalCost = 2000.00m, Status = "Diagnosing" },
                new RepairOrder { DeviceId = 5, ServiceId = 5, TechnicianId = 5, TotalCost = 5000.00m, Status = "Waiting" }
            };
            context.RepairOrders.AddRange(repairOrders);
            context.SaveChanges();

            var users = new User[]
            {
                new User { Username = "admin", Email = "admin@technofix.com", Password = "admin123", Role = "Admin" },
                new User { Username = "technician1", Email = "tech1@technofix.com", Password = "tech123", Role = "Technician", TechnicianId = 1 },
                new User { Username = "technician2", Email = "tech2@technofix.com", Password = "tech123", Role = "Technician", TechnicianId = 2 },
                new User { Username = "user1", Email = "user1@mail.com", Password = "user123", Role = "User" }
            };
            context.Users.AddRange(users);
            context.SaveChanges();
        }
    }
}