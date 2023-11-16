using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions
{
    public  interface IDishRepository
    {
        Task<Dish> CreateDish(Dish dish);
        Task DeleteDish(int dishId);
        Task GetDishById(int dishId);
        Task<List<Dish>> GetAllDishes();
    }
}
