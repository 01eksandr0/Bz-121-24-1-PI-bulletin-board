using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BulletinBoard.DAL.EF
{
    /// <summary>
    /// Потрібна лише інструменту dotnet-ef, щоб створювати міграції
    /// (без запуску самої програми).
    /// </summary>
    public class BoardContextDesignFactory : IDesignTimeDbContextFactory<BoardContext>
    {
        public BoardContext CreateDbContext(string[] args)
        {
            var options = new DbContextOptionsBuilder<BoardContext>()
                .UseSqlite("Data Source=board.db")
                .Options;
            return new BoardContext(options);
        }
    }
}
