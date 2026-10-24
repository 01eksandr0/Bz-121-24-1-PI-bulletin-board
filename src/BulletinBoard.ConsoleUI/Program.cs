using System;
using System.IO;
using System.Text;
using BulletinBoard.BLL.Services;
using BulletinBoard.DAL.Abstractions.Interfaces;
using BulletinBoard.DAL.EF;

namespace BulletinBoard.ConsoleUI
{
    public static class Program
    {
        public static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            // Точка збирання: тільки тут видно, яке джерело даних використовується.
            // Щоб перейти на інше джерело, достатньо замінити цей рядок.
            string databasePath = Path.Combine(AppContext.BaseDirectory, "board.db");
            using (IUnitOfWork unitOfWork = EfUnitOfWorkFactory.CreateSqlite(databasePath))
            {
                var app = new BoardConsole(
                    new UserService(unitOfWork),
                    new CategoryService(unitOfWork),
                    new TagService(unitOfWork),
                    new AdService(unitOfWork));
                app.Run();
            }
        }
    }
}
