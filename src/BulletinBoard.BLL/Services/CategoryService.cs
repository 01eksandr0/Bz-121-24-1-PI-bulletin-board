using System;
using System.Collections.Generic;
using System.Linq;
using BulletinBoard.BLL.Exceptions;
using BulletinBoard.BLL.Interfaces;
using BulletinBoard.BLL.Internal;
using BulletinBoard.BLL.Mapping;
using BulletinBoard.BLL.Models;
using BulletinBoard.DAL.Abstractions.Entities;
using BulletinBoard.DAL.Abstractions.Interfaces;

namespace BulletinBoard.BLL.Services
{
    public class CategoryService : ICategoryService
    {
        /// <summary>Рубрика - категорія - підкатегорія: три рівні.</summary>
        public const int MaxLevels = 3;

        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        private IRepository<Category> Categories => _unitOfWork.Repository<Category>();

        public IReadOnlyList<CategoryModel> GetTree()
        {
            var hierarchy = new CategoryHierarchy(Categories.GetAll());
            var result = new List<CategoryModel>();
            AddBranch(hierarchy, null, 0, result);
            return result;
        }

        public CategoryModel Create(string name, int? parentId)
        {
            string cleanName = (name ?? string.Empty).Trim();
            if (cleanName.Length == 0)
            {
                throw new ValidationFailedException("Назва категорії не може бути порожньою.");
            }

            var hierarchy = new CategoryHierarchy(Categories.GetAll());
            int level = 0;
            if (parentId.HasValue)
            {
                if (!hierarchy.Contains(parentId.Value))
                {
                    throw new NotFoundException("Батьківську категорію", parentId.Value);
                }
                level = hierarchy.GetLevel(parentId.Value) + 1;
                if (level >= MaxLevels)
                {
                    throw new ValidationFailedException("Підкатегорія - останній рівень, в неї не можна додати вкладену категорію.");
                }
            }
            if (hierarchy.GetChildren(parentId).Any(c => string.Equals(c.Name, cleanName, StringComparison.OrdinalIgnoreCase)))
            {
                throw new ValidationFailedException("Категорія \"" + cleanName + "\" на цьому рівні вже існує.");
            }

            var category = new Category { Name = cleanName, ParentId = parentId };
            Categories.Add(category);
            _unitOfWork.SaveChanges();
            return category.ToModel(level);
        }

        private static void AddBranch(CategoryHierarchy hierarchy, int? parentId, int level, List<CategoryModel> result)
        {
            foreach (Category category in hierarchy.GetChildren(parentId))
            {
                result.Add(category.ToModel(level));
                AddBranch(hierarchy, category.Id, level + 1, result);
            }
        }
    }
}
