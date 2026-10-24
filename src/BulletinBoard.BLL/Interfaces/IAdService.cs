using System.Collections.Generic;
using BulletinBoard.BLL.Models;

namespace BulletinBoard.BLL.Interfaces
{
    public interface IAdService
    {
        AdModel Create(int authorId, AdInputModel input);

        AdModel Update(int adId, int userId, AdInputModel input);

        AdModel GetById(int adId, int? viewerId);

        IReadOnlyList<AdModel> Search(AdSearchFilter filter);

        IReadOnlyList<AdModel> GetByAuthor(int authorId);

        void Deactivate(int adId, int userId);

        void Activate(int adId, int userId);

        void Delete(int adId, int userId);
    }
}
