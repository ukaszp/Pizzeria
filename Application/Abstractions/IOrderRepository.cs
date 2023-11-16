using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions
{
    public interface IOrderRepository
    {
        Task<Order> CreateDOrder(Order order);
        Task DeleteOrder(int orderId);
        Task GetOrderById(int orderId);
        Task<List<Order>> GetAllorders();
        Task<Order> ChangeStatus(int oderId, bool status);
    }
}
