using Pizzeria.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Dish
    {
        public int Id { get; set; }
        public string Name { get; set;}
        public string ImageUrl { get; set;}
        public string Description { get; set;}
        public int? SizeInCm { get; set; }
        public float Price { get; set; }
    }
}
