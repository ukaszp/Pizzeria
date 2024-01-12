using AccountApi.Entities;
using AccountApi.Services;
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
        public ActionResult<Order> CreateOrder([FromBody] IEnumerable<int> dishIds, int addressId, int pizzeriaUserId)
        {
            try
            {
                var result = pizzeriaService.CreateOrder(dishIds, addressId, pizzeriaUserId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("dishes")]
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
        [HttpDelete("dishes/{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            pizzeriaService.DeleteDish(id);

            return NoContent();
        }
        [HttpGet("dishes/{id}")]
        public ActionResult<User> Get([FromRoute] int id)
        {
            var dish = pizzeriaService.GetDishById(id);

            return Ok(dish);
        }

        [HttpPost("changeorderstatus/{orderId}")]
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
