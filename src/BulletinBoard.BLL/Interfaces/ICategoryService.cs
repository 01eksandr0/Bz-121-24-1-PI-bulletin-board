using System.Collections.Generic;
using BulletinBoard.BLL.Models;

namespace BulletinBoard.BLL.Interfaces
{
    public interface ICategoryService
    {
        IReadOnlyList<CategoryModel> GetTree();

        CategoryModel Create(string name, int? parentId);
    }
}
