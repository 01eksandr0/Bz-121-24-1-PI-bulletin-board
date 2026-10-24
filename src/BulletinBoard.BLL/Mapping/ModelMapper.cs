using System.Collections.Generic;
using System.Linq;
using BulletinBoard.BLL.Models;
using BulletinBoard.DAL.Abstractions.Entities;

namespace BulletinBoard.BLL.Mapping
{
    /// <summary>
    /// Mapping між сутностями DAL і моделями BLL.
    /// Завдяки йому сутності DAL не виходять за межі бізнес-логіки.
    /// </summary>
    public static class ModelMapper
    {
        public static UserModel ToModel(this User user)
        {
            return new UserModel { Id = user.Id, Name = user.Name, Email = user.Email };
        }

        public static TagModel ToModel(this Tag tag)
        {
            return new TagModel { Id = tag.Id, Name = tag.Name };
        }

        public static CategoryModel ToModel(this Category category, int level)
        {
            return new CategoryModel
            {
                Id = category.Id,
                Name = category.Name,
                ParentId = category.ParentId,
                Level = level
            };
        }

        /// <summary>Потребує підвантажених Author, Category і Tags.</summary>
        public static AdModel ToModel(this Ad ad)
        {
            return new AdModel
            {
                Id = ad.Id,
                Title = ad.Title,
                Description = ad.Description,
                Price = ad.Price,
                CreatedAt = ad.CreatedAt,
                IsActive = ad.IsActive,
                AuthorId = ad.AuthorId,
                AuthorName = ad.Author.Name,
                CategoryId = ad.CategoryId,
                CategoryName = ad.Category.Name,
                Tags = ad.Tags.Select(t => t.Name).OrderBy(n => n).ToList()
            };
        }

        /// <summary>Переносить введені дані в сутність (і при створенні, і при редагуванні).</summary>
        public static void ApplyTo(this AdInputModel input, Ad ad, IEnumerable<Tag> tags)
        {
            ad.Title = input.Title.Trim();
            ad.Description = (input.Description ?? string.Empty).Trim();
            ad.Price = input.Price;
            ad.CategoryId = input.CategoryId;
            ad.Tags.Clear();
            foreach (Tag tag in tags)
            {
                ad.Tags.Add(tag);
            }
        }
    }
}
