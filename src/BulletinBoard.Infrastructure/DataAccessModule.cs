using Autofac;
using BulletinBoard.DAL.Abstractions.Interfaces;
using BulletinBoard.DAL.EF;

namespace BulletinBoard.Infrastructure
{
    /// <summary>
    /// Реєстрація рівня доступу до даних у контейнері Autofac.
    /// Лише тут вирішується, що IUnitOfWork - це EfUnitOfWork на SQLite.
    /// </summary>
    public class DataAccessModule : Module
    {
        private readonly string _connectionString;

        public DataAccessModule(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void Load(ContainerBuilder builder)
        {
            var options = BoardDatabase.CreateSqliteOptions(_connectionString);

            builder.RegisterInstance(options);

            // один контекст і одна UoW на один HTTP-запит
            builder.RegisterType<BoardContext>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<EfUnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();

            // після побудови контейнера створюємо базу, якщо її ще немає
            builder.RegisterBuildCallback(scope => BoardDatabase.EnsureCreated(options));
        }
    }
}
