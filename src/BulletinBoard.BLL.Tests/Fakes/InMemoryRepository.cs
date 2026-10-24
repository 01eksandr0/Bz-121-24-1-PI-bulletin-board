using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using BulletinBoard.DAL.Abstractions.Entities;
using BulletinBoard.DAL.Abstractions.Interfaces;

namespace BulletinBoard.BLL.Tests.Fakes
{
    /// <summary>
    /// Репозиторій у пам'яті для тестів. includes ігноруються: пов'язані об'єкти
    /// підставляє делегат fillReferences (так само, як їх підвантажив би EF).
    /// </summary>
    public class InMemoryRepository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        private readonly List<TEntity> _items = new List<TEntity>();
        private readonly Action<TEntity> _fillReferences;
        private int _nextId = 1;

        public InMemoryRepository(Action<TEntity> fillReferences = null)
        {
            _fillReferences = fillReferences ?? (entity => { });
        }

        public int Count => _items.Count;

        public TEntity GetById(int id)
        {
            return Items().FirstOrDefault(e => e.Id == id);
        }

        public IReadOnlyList<TEntity> GetAll(params Expression<Func<TEntity, object>>[] includes)
        {
            return Items().ToList();
        }

        public IReadOnlyList<TEntity> Find(Expression<Func<TEntity, bool>> predicate,
            params Expression<Func<TEntity, object>>[] includes)
        {
            return Items().Where(predicate.Compile()).ToList();
        }

        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate,
            params Expression<Func<TEntity, object>>[] includes)
        {
            return Items().FirstOrDefault(predicate.Compile());
        }

        public bool Any(Expression<Func<TEntity, bool>> predicate)
        {
            return Items().Any(predicate.Compile());
        }

        public void Add(TEntity entity)
        {
            entity.Id = _nextId++;
            _items.Add(entity);
        }

        public void Update(TEntity entity)
        {
        }

        public void Remove(TEntity entity)
        {
            _items.Remove(entity);
        }

        private IEnumerable<TEntity> Items()
        {
            foreach (TEntity item in _items)
            {
                _fillReferences(item);
            }
            return _items;
        }
    }
}
