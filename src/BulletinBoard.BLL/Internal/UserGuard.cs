using BulletinBoard.BLL.Exceptions;
using BulletinBoard.DAL.Abstractions.Entities;
using BulletinBoard.DAL.Abstractions.Interfaces;

namespace BulletinBoard.BLL.Internal
{
    /// <summary>Перевірка, що дію виконує зареєстрований користувач.</summary>
    internal static class UserGuard
    {
        public static void EnsureRegistered(IUnitOfWork unitOfWork, int userId)
        {
            if (unitOfWork.Repository<User>().GetById(userId) == null)
            {
                throw new AccessDeniedException("Цю дію може виконати тільки зареєстрований користувач.");
            }
        }
    }
}
