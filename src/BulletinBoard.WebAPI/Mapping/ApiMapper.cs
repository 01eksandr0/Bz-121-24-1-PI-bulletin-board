using System.Collections.Generic;
using System.Linq;
using BulletinBoard.BLL.Models;
using BulletinBoard.WebAPI.Models.Requests;
using BulletinBoard.WebAPI.Models.Responses;

namespace BulletinBoard.WebAPI.Mapping
{
    /// <summary>Mapping між моделями PL (запити/відповіді) і моделями BLL.</summary>
    public static class ApiMapper
    {
        public static UserResponse ToResponse(this UserModel user)
        {
            return new UserResponse { Id = user.Id, Name = user.Name, Email = user.Email };
        }

        public static CategoryResponse ToResponse(this CategoryModel category)
        {
            return new CategoryResponse
            {
                Id = category.Id,
                Name = category.Name,
                ParentId = category.ParentId,
                Level = category.Level
            };
        }

        public static TagResponse ToResponse(this TagModel tag)
        {
            return new TagResponse { Id = tag.Id, Name = tag.Name };
        }

        public static AdResponse ToResponse(this AdModel ad)
        {
            return new AdResponse
            {
                Id = ad.Id,
                Title = ad.Title,
                Description = ad.Description,
                Price = ad.Price,
                CreatedAt = ad.CreatedAt,
                IsActive = ad.IsActive,
                Author = new ReferenceResponse { Id = ad.AuthorId, Name = ad.AuthorName },
                Category = new ReferenceResponse { Id = ad.CategoryId, Name = ad.CategoryName },
                Tags = ad.Tags
            };
        }

        public static AdInputModel ToInput(this AdRequest request)
        {
            return new AdInputModel
            {
                Title = request.Title,
                Description = request.Description,
                Price = request.Price.Value,
                CategoryId = request.CategoryId.Value,
                TagIds = request.TagIds ?? new List<int>()
            };
        }

        public static AdSearchFilter ToFilter(this AdSearchQuery query)
        {
            return new AdSearchFilter
            {
                CategoryId = query.CategoryId,
                TagId = query.TagId,
                Text = query.Text
            };
        }
    }
}
