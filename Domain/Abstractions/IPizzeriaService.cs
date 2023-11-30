using Domain.Models;

namespace Pizzeria.Service.Services
{
    public interface IPizzeriaService
    {
        Address AddAddress(Address address);
        Dish AddDish(Dish dish);
        void ChangeOrderStatus(int orderid);
        void CreateDishesList(int dishId);
        Order CreateOrder(IEnumerable<Dish> dishes, Address address, int userId);
        IEnumerable<Dish> GetDishes();
    }
}