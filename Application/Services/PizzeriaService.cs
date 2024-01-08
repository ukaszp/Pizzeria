using AccountApi.Exceptions;
using AccountApi.Services;
using DataAccess;
using Domain.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Pizzeria.Service.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

namespace Pizzeria.Service.Services
{
    public class PizzeriaService : IPizzeriaService
    {
        private readonly IPizzeriaUserService pizzeriaUserService;
        private readonly IRoleService roleService;
        private readonly PizzeriaDbContext dbContext;

        public PizzeriaService(IPizzeriaUserService pizzeriaUserService, IRoleService roleService, PizzeriaDbContext dbContext)
        {
            this.pizzeriaUserService = pizzeriaUserService;
            this.roleService = roleService;
            this.dbContext = dbContext;
        }

        public Dish AddDish(Dish dish)
        {
            var newDish = new Dish()
            {
                Name = dish.Name,
                Description = dish.Description,
                SizeInCm = dish.SizeInCm,
                Price = dish.Price
            };
            dbContext.Dishes.Add(newDish);
            dbContext.SaveChanges();
            return newDish;
        }

        public Address AddAddress(Address address)
        {
            var newAddress = new Address()
            {
                UserId = address.UserId,
                Street = address.Street,
                HomeNumber = address.HomeNumber,
                LocalNumber = address.LocalNumber,
                City = address.City,
                PostalCode = address.PostalCode
            };

            dbContext.Addresses.Add(newAddress);
            dbContext.SaveChanges();
            return newAddress;
        }

        public Order CreateOrder(IEnumerable<Dish> dishes, Address address, int userId)
        {

            var newOrder = new Order()
            {
                UserId = userId,
                Dishes = dishes,
                TotalPrice = dishes.Sum(d => d.Price),
                Address = address,
                WhenOrdered = DateTime.Now,
                IsDone = false
            };

            return newOrder;
        }

        public IEnumerable<Dish> GetDishes()
        {
            var dishes = dbContext
                .Dishes
                .ToList();

            return dishes;
        }

        public void ChangeOrderStatus(int orderid)
        {
            var order = dbContext
                .Orders
                .FirstOrDefault(order => order.Id == orderid);

            if (order != null)
            {
                order.IsDone = true;
            }
            else
            {
                throw new NotFoundException("order not found");
            }

        }
    }
}
