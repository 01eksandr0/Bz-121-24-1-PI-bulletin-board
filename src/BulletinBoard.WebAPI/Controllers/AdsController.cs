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
    /// <summary>
    /// Оголошення. Контролер тільки приймає запит, перетворює моделі PL у моделі BLL,
    /// викликає сервіс і повертає HTTP-відповідь. Правила (хто автор, що допустимо) - у BLL.
    /// </summary>
    [ApiController]
    [Route("api/ads")]
    public class AdsController : ControllerBase
    {
        private readonly IAdService _ads;

        public AdsController(IAdService ads)
        {
            _ads = ads;
        }

        /// <summary>GET api/ads?categoryId=1&amp;tagId=2&amp;text=... - пошук активних оголошень.</summary>
        [HttpGet]
        public ActionResult<List<AdResponse>> Search([FromQuery] AdSearchQuery query)
        {
            return _ads.Search(query.ToFilter()).Select(a => a.ToResponse()).ToList();
        }

        /// <summary>GET api/ads/5 - неактивне оголошення побачить тільки автор (за X-User-Id).</summary>
        [HttpGet("{id:int}")]
        public ActionResult<AdResponse> GetById(int id, [FromHeader(Name = ApiHeaders.UserId)] int? userId)
        {
            return _ads.GetById(id, userId).ToResponse();
        }

        /// <summary>GET api/ads/my - усі оголошення поточного користувача, разом з неактивними.</summary>
        [HttpGet("my")]
        [RequireUser]
        public ActionResult<List<AdResponse>> GetMine([FromHeader(Name = ApiHeaders.UserId)] int userId)
        {
            return _ads.GetByAuthor(userId).Select(a => a.ToResponse()).ToList();
        }

        /// <summary>POST api/ads - нове оголошення від імені X-User-Id.</summary>
        [HttpPost]
        [RequireUser]
        public ActionResult<AdResponse> Create(
            [FromHeader(Name = ApiHeaders.UserId)] int userId,
            [FromBody] AdRequest request)
        {
            AdResponse ad = _ads.Create(userId, request.ToInput()).ToResponse();
            return CreatedAtAction(nameof(GetById), new { id = ad.Id }, ad);
        }

        /// <summary>PUT api/ads/5 - повна заміна даних оголошення (тільки автор).</summary>
        [HttpPut("{id:int}")]
        [RequireUser]
        public ActionResult<AdResponse> Update(
            int id,
            [FromHeader(Name = ApiHeaders.UserId)] int userId,
            [FromBody] AdRequest request)
        {
            return _ads.Update(id, userId, request.ToInput()).ToResponse();
        }

        /// <summary>POST api/ads/5/deactivate (тільки автор).</summary>
        [HttpPost("{id:int}/deactivate")]
        [RequireUser]
        public IActionResult Deactivate(int id, [FromHeader(Name = ApiHeaders.UserId)] int userId)
        {
            _ads.Deactivate(id, userId);
            return NoContent();
        }

        /// <summary>POST api/ads/5/activate (тільки автор).</summary>
        [HttpPost("{id:int}/activate")]
        [RequireUser]
        public IActionResult Activate(int id, [FromHeader(Name = ApiHeaders.UserId)] int userId)
        {
            _ads.Activate(id, userId);
            return NoContent();
        }

        /// <summary>DELETE api/ads/5 (тільки автор).</summary>
        [HttpDelete("{id:int}")]
        [RequireUser]
        public IActionResult Delete(int id, [FromHeader(Name = ApiHeaders.UserId)] int userId)
        {
            _ads.Delete(id, userId);
            return NoContent();
        }
    }
}
