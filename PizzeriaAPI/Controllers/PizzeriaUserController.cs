using AccountApi.Entities;
using AccountApi.Models;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Pizzeria.Service.Abstractions;

namespace PizzeriaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PizzeriaUserController : ControllerBase
    {
        private readonly IPizzeriaUserService pizzeriaUserService;

        public PizzeriaUserController(IPizzeriaUserService pizzeriaUserService)
        {
            this.pizzeriaUserService = pizzeriaUserService;
        }

        [HttpPost("RegisterUser")]
        public ActionResult<PizzeriaUser> RegisterUser([FromBody] CreateUserDto dto)
        {
            
                var result = pizzeriaUserService.RegisterUser(dto);
                return Ok(result);
        }

        [HttpGet("GetAccountInfo/{pizzeriaUserId}")]
        public ActionResult<User> GetAccountInfo(int pizzeriaUserId)
        {
            try
            {
                var result = pizzeriaUserService.GetAccountInfo(pizzeriaUserId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetAllUserOrders/{UserId}")]
        public ActionResult<IEnumerable<Order>> GetAllUserOrders(int UserId)
        {
            try
            {
                var result = pizzeriaUserService.GetAllUserOrders(UserId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetAllAccounts")]
        public ActionResult<IEnumerable<User>> GetAllAccounts()
        {
            try
            {
                var result = pizzeriaUserService.GetAllAccounts();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteUser/{pizzeriaUserId}")]
        public ActionResult DeleteUser(int pizzeriaUserId)
        {
            try
            {
                pizzeriaUserService.DeleteUser(pizzeriaUserId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("AssignRole/{roleId}/{pizzeriaUserId}")]
        public ActionResult AssignRole(int roleId, int pizzeriaUserId)
        {
            try
            {
                pizzeriaUserService.AssignRole(roleId, pizzeriaUserId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetAccountById/{pizzeriaUserId}")]
        public ActionResult<User> GetAccountById(int pizzeriaUserId)
        {
            try
            {
                var result = pizzeriaUserService.GetAccountById(pizzeriaUserId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("GetJwtToken")]
        public ActionResult GetJwtToken([FromBody] LoginDto dto)
        {
            try
            {
                pizzeriaUserService.GetJwtToken(dto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("GetUserLogin")]
        public ActionResult<User> GetUserLogin([FromBody] LoginDto dto)
        {
            try
            {
                var result = pizzeriaUserService.GetUserLogin(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
