using System.Collections.Generic;
using System.Linq;
using BulletinBoard.BLL.Interfaces;
using BulletinBoard.WebAPI.Mapping;
using BulletinBoard.WebAPI.Models.Requests;
using BulletinBoard.WebAPI.Models.Responses;
using Microsoft.AspNetCore.Mvc;

namespace BulletinBoard.WebAPI.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _users;

        public UsersController(IUserService users)
        {
            _users = users;
        }

        /// <summary>GET api/users - усі користувачі; GET api/users?email=... - пошук за email (вхід).</summary>
        [HttpGet]
        public ActionResult<List<UserResponse>> Get([FromQuery] string email)
        {
            if (!string.IsNullOrWhiteSpace(email))
            {
                return new List<UserResponse> { _users.GetByEmail(email).ToResponse() };
            }
            return _users.GetAll().Select(u => u.ToResponse()).ToList();
        }

        /// <summary>GET api/users/5</summary>
        [HttpGet("{id:int}")]
        public ActionResult<UserResponse> GetById(int id)
        {
            return _users.GetById(id).ToResponse();
        }

        /// <summary>POST api/users - реєстрація.</summary>
        [HttpPost]
        public ActionResult<UserResponse> Register([FromBody] RegisterUserRequest request)
        {
            UserResponse user = _users.Register(request.Name, request.Email).ToResponse();
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }
    }
}
