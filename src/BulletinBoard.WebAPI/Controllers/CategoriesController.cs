using System.Collections.Generic;
using System.Linq;
using BulletinBoard.BLL.Interfaces;
using BulletinBoard.WebAPI.Filters;
using BulletinBoard.WebAPI.Mapping;
using BulletinBoard.WebAPI.Models.Requests;
using BulletinBoard.WebAPI.Models.Responses;
using Microsoft.AspNetCore.Mvc;

namespace BulletinBoard.WebAPI.Controllers
{
    [ApiController]
    [Route("api/categories")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categories;

        public CategoriesController(ICategoryService categories)
        {
            _categories = categories;
        }

        /// <summary>GET api/categories - дерево категорій (обхід у глибину, Level - рівень).</summary>
        [HttpGet]
        public ActionResult<List<CategoryResponse>> GetTree()
        {
            return _categories.GetTree().Select(c => c.ToResponse()).ToList();
        }

        /// <summary>POST api/categories - нова рубрика, категорія чи підкатегорія.</summary>
        [HttpPost]
        [RequireUser]
        public ActionResult<CategoryResponse> Create(
            [FromHeader(Name = ApiHeaders.UserId)] int userId,
            [FromBody] CreateCategoryRequest request)
        {
            CategoryResponse category = _categories.Create(userId, request.Name, request.ParentId).ToResponse();
            return Created("/api/categories", category);
        }
    }
}
