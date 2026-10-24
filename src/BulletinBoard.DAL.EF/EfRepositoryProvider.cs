using System;
using System.Collections.Generic;
using BulletinBoard.DAL.Abstractions.Entities;
using BulletinBoard.DAL.Abstractions.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BulletinBoard.DAL.EF
{
    /// <summary>
    /// Створює і кешує репозиторії для одного контексту.
    /// Винесено з UnitOfWork, щоб UoW займався тільки збереженням змін.
    /// </summary>
    public class EfRepositoryProvider
    {
        private readonly DbContext _context;
        private readonly Dictionary<Type, object> _repositories = new Dictionary<Type, object>();

        public EfRepositoryProvider(DbContext context)
        {
            _context = context;
        }

        public IRepository<TEntity> Get<TEntity>() where TEntity : class, IEntity
        {
            object repository;
            if (!_repositories.TryGetValue(typeof(TEntity), out repository))
            {
                repository = new EfRepository<TEntity>(_context);
                _repositories.Add(typeof(TEntity), repository);
            }
            return (IRepository<TEntity>)repository;
        }
    }
}
