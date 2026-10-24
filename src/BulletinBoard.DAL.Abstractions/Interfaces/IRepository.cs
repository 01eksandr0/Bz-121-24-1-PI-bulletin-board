using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using BulletinBoard.DAL.Abstractions.Entities;

namespace BulletinBoard.DAL.Abstractions.Interfaces
{
    /// <summary>
    /// Один generic-репозиторій для всіх операцій з усіма сутностями.
    /// includes - які пов'язані дані підвантажити (джерело, яке зберігає
    /// дані разом, може їх просто ігнорувати).
    /// </summary>
    public interface IRepository<TEntity> where TEntity : class, IEntity
    {
        TEntity GetById(int id);

        IReadOnlyList<TEntity> GetAll(params Expression<Func<TEntity, object>>[] includes);

        IReadOnlyList<TEntity> Find(
            Expression<Func<TEntity, bool>> predicate,
            params Expression<Func<TEntity, object>>[] includes);

        TEntity FirstOrDefault(
            Expression<Func<TEntity, bool>> predicate,
            params Expression<Func<TEntity, object>>[] includes);

        bool Any(Expression<Func<TEntity, bool>> predicate);

        void Add(TEntity entity);

        void Update(TEntity entity);

        void Remove(TEntity entity);
    }
}
