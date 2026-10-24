using System;
using System.Collections.Generic;
using System.Linq;
using BulletinBoard.BLL.Exceptions;
using BulletinBoard.BLL.Interfaces;
using BulletinBoard.BLL.Mapping;
using BulletinBoard.BLL.Models;
using BulletinBoard.DAL.Abstractions.Entities;
using BulletinBoard.DAL.Abstractions.Interfaces;

namespace BulletinBoard.BLL.Services
{
    public class TagService : ITagService
    {
        private const int MaxNameLength = 50;

        private readonly IUnitOfWork _unitOfWork;

        public TagService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        private IRepository<Tag> Tags => _unitOfWork.Repository<Tag>();

        public IReadOnlyList<TagModel> GetAll()
        {
            return Tags.GetAll().OrderBy(t => t.Name).Select(t => t.ToModel()).ToList();
        }

        public TagModel Create(string name)
        {
            string cleanName = (name ?? string.Empty).Trim().ToLowerInvariant();
            if (cleanName.Length == 0 || cleanName.Length > MaxNameLength)
            {
                throw new ValidationFailedException("Назва тегу має бути від 1 до " + MaxNameLength + " символів.");
            }
            if (Tags.GetAll().Any(t => string.Equals(t.Name, cleanName, StringComparison.OrdinalIgnoreCase)))
            {
                throw new ValidationFailedException("Тег \"" + cleanName + "\" вже існує.");
            }

            var tag = new Tag { Name = cleanName };
            Tags.Add(tag);
            _unitOfWork.SaveChanges();
            return tag.ToModel();
        }
    }
}
