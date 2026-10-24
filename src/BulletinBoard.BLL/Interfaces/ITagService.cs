using System.Collections.Generic;
using BulletinBoard.BLL.Models;

namespace BulletinBoard.BLL.Interfaces
{
    public interface ITagService
    {
        IReadOnlyList<TagModel> GetAll();

        TagModel Create(int userId, string name);
    }
}
