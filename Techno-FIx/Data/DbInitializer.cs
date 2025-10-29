using Techno_FIx.Models;

namespace Techno_FIx.Data
{
    public static class DbInitializer
    {
        public static void Initialize(AppDbContext context)
        {
            context.Database.EnsureCreated();

            
            if (context.Clients.Any())
            {
                return; 
            }


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
        }
    }
}
