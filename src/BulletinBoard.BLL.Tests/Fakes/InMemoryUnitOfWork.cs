using System;
using System.Collections.Generic;
using System.Linq;
using BulletinBoard.DAL.Abstractions.Entities;
using BulletinBoard.DAL.Abstractions.Interfaces;

namespace BulletinBoard.BLL.Tests.Fakes
{
    /// <summary>UnitOfWork у пам'яті: дозволяє тестувати BLL без бази даних.</summary>
    public class InMemoryUnitOfWork : IUnitOfWork
    {
        private readonly Dictionary<Type, object> _repositories = new Dictionary<Type, object>();

        public InMemoryUnitOfWork()
        {
            Users = new InMemoryRepository<User>();
            Categories = new InMemoryRepository<Category>();
            Tags = new InMemoryRepository<Tag>();
            Ads = new InMemoryRepository<Ad>(ad =>
            {
                ad.Author = Users.GetById(ad.AuthorId);
                ad.Category = Categories.GetById(ad.CategoryId);
            });
            _repositories.Add(typeof(User), Users);
            _repositories.Add(typeof(Category), Categories);
            _repositories.Add(typeof(Tag), Tags);
            _repositories.Add(typeof(Ad), Ads);
        }

        public InMemoryRepository<User> Users { get; }

        public InMemoryRepository<Category> Categories { get; }

        public InMemoryRepository<Tag> Tags { get; }

        public InMemoryRepository<Ad> Ads { get; }

        public int SaveCount { get; private set; }

        public IRepository<TEntity> Repository<TEntity>() where TEntity : class, IEntity
        {
            return (IRepository<TEntity>)_repositories[typeof(TEntity)];
        }

        public void SaveChanges()
        {
            SaveCount++;
        }

        public void Dispose()
        {
        }
    }
}
