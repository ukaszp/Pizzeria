using AccountApi.Entities;
using AccountApi.Exceptions;
using AccountApi.Services;
using DataAccess;
using Domain.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Pizzeria.Domain.Models;
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
                ImageUrl = dish.ImageUrl,
                Description = dish.Description,
                SizeInCm = dish.SizeInCm,
                Price = dish.Price
            };
            dbContext.Dishes.Add(newDish);
            dbContext.SaveChanges();
            return newDish;
        }
        public void DeleteDish(int id)
        {
         
            var dish = dbContext
                .Dishes
                .FirstOrDefault(u => u.Id == id);

            if (dish is null)
                throw new NotFoundException("Dish not found");

            dbContext.Dishes.Remove(dish);
            dbContext.SaveChanges();
        }
        public Dish GetDishById(int id)
        {
            var dish = dbContext
                .Dishes
                .FirstOrDefault(u => u.Id == id);

            return dish == null ? throw new NotFoundException("Dish not found") : dish;
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

        public Order CreateOrder(IEnumerable<int> dishIds, int addressId, int pizzeriaUserId)
        {

            var newOrder = new Order()
            {
                PizzeriaUserId = pizzeriaUserId,
                Dishes = GetDishesByIds(dishIds),
                AddressId = addressId,
                WhenOrdered = DateTime.Now,
                IsDone = false
            };

            float totalPrice = dbContext.Dishes.Where(d => dishIds.Contains(d.Id)).Sum(d => d.Price);
            newOrder.TotalPrice = totalPrice;

            dbContext.Orders.Add(newOrder);
            dbContext.SaveChanges();

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

        private List<Dish> GetDishesByIds(IEnumerable<int> dishIds)
        {           
                return dbContext.Dishes
                    .Where(d => dishIds.Contains(d.Id))
                    .ToList();
        }

    }
}
