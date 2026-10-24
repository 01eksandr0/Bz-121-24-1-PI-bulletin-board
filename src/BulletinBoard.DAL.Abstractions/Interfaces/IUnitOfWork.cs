using System;
using BulletinBoard.DAL.Abstractions.Entities;

namespace BulletinBoard.DAL.Abstractions.Interfaces
{
    /// <summary>
    /// Єдина точка доступу до репозиторіїв і до збереження змін.
    /// Усі зміни, зроблені через репозиторії, записуються разом у SaveChanges.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        IRepository<TEntity> Repository<TEntity>() where TEntity : class, IEntity;

        void SaveChanges();
    }
}
