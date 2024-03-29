﻿using Pizzeria.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public int PizzeriaUserId { get; set; }
        public IEnumerable<Dish> Dishes { get; set; }
        public float TotalPrice { get; set; }
        public int AddressId { get; set; }
        public DateTime WhenOrdered { get; set; }
        public bool IsDone { get; set; }

    }
}
