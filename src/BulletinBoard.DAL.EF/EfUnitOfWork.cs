using BulletinBoard.DAL.Abstractions.Entities;
using BulletinBoard.DAL.Abstractions.Interfaces;

namespace BulletinBoard.DAL.EF
{
    /// <summary>Unit of Work на EF Core: один контекст, одне збереження для всіх змін.</summary>
    public class EfUnitOfWork : IUnitOfWork
    {
        private readonly BoardContext _context;
        private readonly EfRepositoryProvider _repositories;

        public EfUnitOfWork(BoardContext context)
        {
            _context = context;
            _repositories = new EfRepositoryProvider(context);
        }

        public IRepository<TEntity> Repository<TEntity>() where TEntity : class, IEntity
        {
            return _repositories.Get<TEntity>();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
