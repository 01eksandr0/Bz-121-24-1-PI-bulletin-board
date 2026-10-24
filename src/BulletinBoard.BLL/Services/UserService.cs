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
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        private IRepository<User> Users => _unitOfWork.Repository<User>();

        public UserModel Register(string name, string email)
        {
            string cleanName = (name ?? string.Empty).Trim();
            string cleanEmail = NormalizeEmail(email);

            if (cleanName.Length == 0)
            {
                throw new ValidationFailedException("Ім'я не може бути порожнім.");
            }
            if (!cleanEmail.Contains("@") || cleanEmail.StartsWith("@") || cleanEmail.EndsWith("@"))
            {
                throw new ValidationFailedException("Некоректний email.");
            }
            if (Users.Any(u => u.Email == cleanEmail))
            {
                throw new ValidationFailedException("Користувач з таким email вже зареєстрований.");
            }

            var user = new User { Name = cleanName, Email = cleanEmail };
            Users.Add(user);
            _unitOfWork.SaveChanges();
            return user.ToModel();
        }

        public UserModel GetByEmail(string email)
        {
            string cleanEmail = NormalizeEmail(email);
            User user = Users.FirstOrDefault(u => u.Email == cleanEmail);
            if (user == null)
            {
                throw new NotFoundException("Користувача з email " + cleanEmail + " не знайдено.");
            }
            return user.ToModel();
        }

        public IReadOnlyList<UserModel> GetAll()
        {
            return Users.GetAll().OrderBy(u => u.Name).Select(u => u.ToModel()).ToList();
        }

        private static string NormalizeEmail(string email)
        {
            return (email ?? string.Empty).Trim().ToLowerInvariant();
        }
    }
}
