using Microsoft.EntityFrameworkCore;

namespace BulletinBoard.DAL.EF
{
    /// <summary>
    /// Налаштування підключення до SQLite і створення бази.
    /// Використовується і консоллю (через EfUnitOfWorkFactory), і WebAPI (через DI-модуль).
    /// </summary>
    public static class BoardDatabase
    {
        public static DbContextOptions<BoardContext> CreateSqliteOptions(string connectionString)
        {
            return new DbContextOptionsBuilder<BoardContext>()
                .UseSqlite(connectionString)
                .Options;
        }

        /// <summary>Якщо бази ще немає - створює її за моделлю (Code First) і заповнює тестовими даними.</summary>
        public static void EnsureCreated(DbContextOptions<BoardContext> options)
        {
            using (var context = new BoardContext(options))
            {
                if (context.Database.EnsureCreated())
                {
                    DemoDataSeeder.Seed(context);
                }
            }
        }
    }
}
