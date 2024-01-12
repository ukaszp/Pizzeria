using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pizzeria.Domain.Models
{
    public class CreateOrderDto
    {
        public int PizzeriaUserId { get; set; }
        public int AddressId { get; set; }
        public IEnumerable<int> DishIds { get; set; }
    }
}
