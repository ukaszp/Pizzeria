using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Pizzeria.Service.Services;

namespace YourNamespace.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PizzeriaController : ControllerBase
    {
        private readonly IPizzeriaService pizzeriaService;

        public PizzeriaController(IPizzeriaService pizzeriaService)
        {
            this.pizzeriaService = pizzeriaService;
        }

        [HttpPost("AddDish")]
        public ActionResult<Dish> AddDish([FromBody] Dish dish)
        {
            try
            {
                var result = pizzeriaService.AddDish(dish);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("AddAddress")]
        public ActionResult<Address> AddAddress([FromBody] Address address)
        {
            try
            {
                var result = pizzeriaService.AddAddress(address);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("CreateOrder")]
        public ActionResult<Order> CreateOrder([FromBody] Order requestDto)
        {
            try
            {
                var result = pizzeriaService.CreateOrder(requestDto.Dishes, requestDto.Address, requestDto.UserId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("CreateDishesList/{dishId}")]
        public ActionResult CreateDishesList(int dishId)
        {
            try
            {
                pizzeriaService.CreateDishesList(dishId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetDishes")]
        public ActionResult<IEnumerable<Dish>> GetDishes()
        {
            try
            {
                var result = pizzeriaService.GetDishes();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("ChangeOrderStatus/{orderId}")]
        public ActionResult ChangeOrderStatus(int orderId)
        {
            try
            {
                pizzeriaService.ChangeOrderStatus(orderId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
