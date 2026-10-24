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
    [Route("api/tags")]
    public class TagsController : ControllerBase
    {
        private readonly ITagService _tags;

        public TagsController(ITagService tags)
        {
            _tags = tags;
        }

        /// <summary>GET api/tags</summary>
        [HttpGet]
        public ActionResult<List<TagResponse>> GetAll()
        {
            return _tags.GetAll().Select(t => t.ToResponse()).ToList();
        }

        /// <summary>POST api/tags</summary>
        [HttpPost]
        [RequireUser]
        public ActionResult<TagResponse> Create(
            [FromHeader(Name = ApiHeaders.UserId)] int userId,
            [FromBody] CreateTagRequest request)
        {
            TagResponse tag = _tags.Create(userId, request.Name).ToResponse();
            return Created("/api/tags", tag);
        }
    }
}
