using BulletinBoard.DAL.Abstractions.Interfaces;

namespace BulletinBoard.DAL.EF
{
    /// <summary>Створює готовий до роботи UnitOfWork для SQLite (використовується консольним UI).</summary>
    public static class EfUnitOfWorkFactory
    {
        public static IUnitOfWork CreateSqlite(string databasePath)
        {
            var options = BoardDatabase.CreateSqliteOptions("Data Source=" + databasePath);
            BoardDatabase.EnsureCreated(options);
            return new EfUnitOfWork(new BoardContext(options));
        }
    }
}
