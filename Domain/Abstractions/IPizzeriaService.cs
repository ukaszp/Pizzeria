using Domain.Models;
using Pizzeria.Domain.Models;

namespace Pizzeria.Service.Services
{
    public interface IPizzeriaService
    {
        Address AddAddress(Address address);
        Dish AddDish(Dish dish);
        Dish GetDishById(int id);
        void ChangeOrderStatus(int orderid);
        void DeleteDish(int id);
        Order CreateOrder(IEnumerable<int> dishIds, int addressId, int pizzeriaUserId);
        IEnumerable<Dish> GetDishes();
    }
}