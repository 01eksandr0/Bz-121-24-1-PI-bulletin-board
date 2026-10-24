using System.Collections.Generic;
using BulletinBoard.BLL.Models;

namespace BulletinBoard.BLL.Interfaces
{
    public interface IUserService
    {
        UserModel Register(string name, string email);

        UserModel GetByEmail(string email);

        IReadOnlyList<UserModel> GetAll();
    }
}
