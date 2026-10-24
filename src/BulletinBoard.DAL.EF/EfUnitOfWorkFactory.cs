using BulletinBoard.DAL.Abstractions.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BulletinBoard.DAL.EF
{
    /// <summary>
    /// Створює готовий до роботи UnitOfWork для SQLite.
    /// Якщо бази ще немає - створює її за моделлю (Code First) і заповнює тестовими даними.
    /// </summary>
    public static class EfUnitOfWorkFactory
    {
        public static IUnitOfWork CreateSqlite(string databasePath)
        {
            var options = new DbContextOptionsBuilder<BoardContext>()
                .UseSqlite("Data Source=" + databasePath)
                .Options;

            var context = new BoardContext(options);
            if (context.Database.EnsureCreated())
            {
                DemoDataSeeder.Seed(context);
            }
            return new EfUnitOfWork(context);
        }
    }
}
