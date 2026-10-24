using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using BulletinBoard.BLL.Exceptions;
using BulletinBoard.BLL.Interfaces;
using BulletinBoard.BLL.Internal;
using BulletinBoard.BLL.Mapping;
using BulletinBoard.BLL.Models;
using BulletinBoard.DAL.Abstractions.Entities;
using BulletinBoard.DAL.Abstractions.Interfaces;

namespace BulletinBoard.BLL.Services
{
    public class AdService : IAdService
    {
        public const int MaxTags = 5;
        private const int MinTitleLength = 3;
        private const int MaxTitleLength = 150;
        private const int MaxDescriptionLength = 2000;

        /// <summary>Пов'язані дані, без яких оголошення не можна показати.</summary>
        private static readonly Expression<Func<Ad, object>>[] Details =
        {
            a => a.Author,
            a => a.Category,
            a => a.Tags
        };

        private readonly IUnitOfWork _unitOfWork;

        public AdService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        private IRepository<Ad> Ads => _unitOfWork.Repository<Ad>();

        public AdModel Create(int authorId, AdInputModel input)
        {
            UserGuard.EnsureRegistered(_unitOfWork, authorId);
            List<Tag> tags = ValidateInputAndLoadTags(input);

            var ad = new Ad { AuthorId = authorId, CreatedAt = DateTime.Now, IsActive = true };
            input.ApplyTo(ad, tags);
            Ads.Add(ad);
            _unitOfWork.SaveChanges();
            return LoadWithDetails(ad.Id).ToModel();
        }

        public AdModel Update(int adId, int userId, AdInputModel input)
        {
            Ad ad = GetOwnedAd(adId, userId, "редагувати");
            List<Tag> tags = ValidateInputAndLoadTags(input);

            input.ApplyTo(ad, tags);
            Ads.Update(ad);
            _unitOfWork.SaveChanges();
            return LoadWithDetails(ad.Id).ToModel();
        }

        public AdModel GetById(int adId, int? viewerId)
        {
            Ad ad = LoadWithDetails(adId);
            // неактивне оголошення бачить тільки його автор
            if (ad == null || (!ad.IsActive && ad.AuthorId != viewerId))
            {
                throw new NotFoundException("Оголошення", adId);
            }
            return ad.ToModel();
        }

        public IReadOnlyList<AdModel> Search(AdSearchFilter filter)
        {
            List<int> categoryIds = null;
            if (filter.CategoryId.HasValue)
            {
                var hierarchy = new CategoryHierarchy(_unitOfWork.Repository<Category>().GetAll());
                if (!hierarchy.Contains(filter.CategoryId.Value))
                {
                    throw new NotFoundException("Категорію", filter.CategoryId.Value);
                }
                categoryIds = hierarchy.GetSelfAndDescendantIds(filter.CategoryId.Value);
            }
            // замість перевірок на null використовуються прапорці: так умову
            // однаково добре зрозуміє і EF (переведе в SQL), і будь-яке інше джерело
            bool allCategories = categoryIds == null;
            List<int> ids = categoryIds ?? new List<int>();
            bool anyTag = !filter.TagId.HasValue;
            int tagId = filter.TagId ?? 0;

            IEnumerable<Ad> ads = Ads.Find(a => a.IsActive
                                                && (allCategories || ids.Contains(a.CategoryId))
                                                && (anyTag || a.Tags.Any(t => t.Id == tagId)),
                                           Details);

            // пошук тексту без урахування регістру робиться тут, бо не всі джерела
            // даних (наприклад, SQLite для кирилиці) вміють це робити самі
            string text = (filter.Text ?? string.Empty).Trim();
            if (text.Length > 0)
            {
                ads = ads.Where(a => ContainsIgnoreCase(a.Title, text) || ContainsIgnoreCase(a.Description, text));
            }
            return ToSortedModels(ads);
        }

        public IReadOnlyList<AdModel> GetByAuthor(int authorId)
        {
            return ToSortedModels(Ads.Find(a => a.AuthorId == authorId, Details));
        }

        public void Deactivate(int adId, int userId)
        {
            ChangeActivity(adId, userId, false);
        }

        public void Activate(int adId, int userId)
        {
            ChangeActivity(adId, userId, true);
        }

        public void Delete(int adId, int userId)
        {
            Ad ad = GetOwnedAd(adId, userId, "видалити");
            Ads.Remove(ad);
            _unitOfWork.SaveChanges();
        }

        private void ChangeActivity(int adId, int userId, bool active)
        {
            Ad ad = GetOwnedAd(adId, userId, active ? "активувати" : "деактивувати");
            if (ad.IsActive == active)
            {
                throw new ValidationFailedException(active ? "Оголошення вже активне." : "Оголошення вже деактивоване.");
            }
            ad.IsActive = active;
            Ads.Update(ad);
            _unitOfWork.SaveChanges();
        }

        /// <summary>Повертає оголошення, тільки якщо userId - його автор.</summary>
        private Ad GetOwnedAd(int adId, int userId, string action)
        {
            Ad ad = LoadWithDetails(adId);
            if (ad == null)
            {
                throw new NotFoundException("Оголошення", adId);
            }
            if (ad.AuthorId != userId)
            {
                throw new AccessDeniedException("Тільки автор може " + action + " оголошення.");
            }
            return ad;
        }

        private Ad LoadWithDetails(int adId)
        {
            return Ads.FirstOrDefault(a => a.Id == adId, Details);
        }

        private List<Tag> ValidateInputAndLoadTags(AdInputModel input)
        {
            string title = (input.Title ?? string.Empty).Trim();
            if (title.Length < MinTitleLength || title.Length > MaxTitleLength)
            {
                throw new ValidationFailedException("Заголовок має бути від " + MinTitleLength + " до " + MaxTitleLength + " символів.");
            }
            if ((input.Description ?? string.Empty).Length > MaxDescriptionLength)
            {
                throw new ValidationFailedException("Опис не може бути довшим за " + MaxDescriptionLength + " символів.");
            }
            if (input.Price < 0)
            {
                throw new ValidationFailedException("Ціна не може бути від'ємною.");
            }
            if (_unitOfWork.Repository<Category>().GetById(input.CategoryId) == null)
            {
                throw new NotFoundException("Категорію", input.CategoryId);
            }

            List<int> tagIds = (input.TagIds ?? new List<int>()).Distinct().ToList();
            if (tagIds.Count > MaxTags)
            {
                throw new ValidationFailedException("Можна вказати не більше " + MaxTags + " тегів.");
            }
            List<Tag> tags = _unitOfWork.Repository<Tag>().Find(t => tagIds.Contains(t.Id)).ToList();
            List<int> missing = tagIds.Where(id => tags.All(t => t.Id != id)).ToList();
            if (missing.Count > 0)
            {
                throw new NotFoundException("Тег", missing[0]);
            }
            return tags;
        }

        private static IReadOnlyList<AdModel> ToSortedModels(IEnumerable<Ad> ads)
        {
            return ads.OrderByDescending(a => a.CreatedAt).ThenByDescending(a => a.Id)
                      .Select(a => a.ToModel()).ToList();
        }

        private static bool ContainsIgnoreCase(string source, string value)
        {
            return source != null && source.IndexOf(value, StringComparison.CurrentCultureIgnoreCase) >= 0;
        }
    }
}
