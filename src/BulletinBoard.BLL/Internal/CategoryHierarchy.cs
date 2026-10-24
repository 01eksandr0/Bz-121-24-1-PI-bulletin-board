using System.Collections.Generic;
using System.Linq;
using BulletinBoard.DAL.Abstractions.Entities;

namespace BulletinBoard.BLL.Internal
{
    /// <summary>
    /// Робота з деревом категорій: рівень вкладеності, усі підкатегорії, обхід дерева.
    /// Використовується і CategoryService, і AdService, тому винесено окремо (DRY).
    /// </summary>
    internal class CategoryHierarchy
    {
        private readonly Dictionary<int, Category> _byId;
        private readonly ILookup<int?, Category> _byParent;

        public CategoryHierarchy(IEnumerable<Category> categories)
        {
            var list = categories.ToList();
            _byId = list.ToDictionary(c => c.Id);
            _byParent = list.ToLookup(c => c.ParentId);
        }

        public bool Contains(int id)
        {
            return _byId.ContainsKey(id);
        }

        /// <summary>0 - рубрика, 1 - категорія, 2 - підкатегорія.</summary>
        public int GetLevel(int id)
        {
            int level = 0;
            int? parentId = _byId[id].ParentId;
            while (parentId.HasValue)
            {
                level++;
                parentId = _byId[parentId.Value].ParentId;
            }
            return level;
        }

        public IEnumerable<Category> GetChildren(int? parentId)
        {
            return _byParent[parentId].OrderBy(c => c.Name);
        }

        /// <summary>Сама категорія і всі вкладені в неї.</summary>
        public List<int> GetSelfAndDescendantIds(int id)
        {
            var result = new List<int> { id };
            foreach (Category child in GetChildren(id))
            {
                result.AddRange(GetSelfAndDescendantIds(child.Id));
            }
            return result;
        }
    }
}
