using AutoMapper;
using AutoMapper.Configuration.Conventions;
using AccountApi.Entities;
using AccountApi.Models;
using AccountApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace AccountApi.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService userService;

        public AccountController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<User>> GetAllSearch([FromQuery]string search)
        {
            var users = userService.GetAllSearch(search);

            return Ok(users);
        }
        [HttpGet("all")]
        public ActionResult<IEnumerable<User>> Get()
        {
            var users = userService.GetAll();

            return Ok(users);
        }


        [HttpPost("register")]
        public ActionResult CreateUser([FromBody] CreateUserDto dto)
        {
            userService.CreateUser(dto);
            return Ok();
        }

        [HttpPost("login")]
        public ActionResult Login([FromBody] LoginDto dto)
        {
            string token = userService.GenerateJwt(dto);
            User user = userService.GetUserLogin(dto);
            var result = new
            {
                Token = token,
                User = user
            };
            return Ok(result);
        }

        [HttpGet("{id}")]
        public ActionResult<User> Get([FromRoute]int id)
        {
            var user=userService.GetById(id);

           return Ok(user);
        }


        [HttpPut("{id}")]
        public ActionResult Update([FromRoute] int id, [FromBody] UpdateUserDto user)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }
            var isUpdated = userService.UpdateUser(id, user);


            return isUpdated ? Ok(): NotFound();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute]int id)
        {
            userService.DeleteUser(id);

            return NoContent();
        }   
        [HttpPut("assignrole/{roleid}/{userid}")]
        public ActionResult AssignRole([FromRoute]int  roleid, [FromRoute]int userid) 
        {
            userService.AssignRole(roleid, userid);
            return Ok();
        }

    }
}
