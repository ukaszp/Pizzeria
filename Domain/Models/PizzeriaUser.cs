using AccountApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class PizzeriaUser
    {
        public int Id { get; set; }
        public int ServiceUserId { get; set; }
        public IEnumerable<Order> Orders { get; set; }
    }
}
