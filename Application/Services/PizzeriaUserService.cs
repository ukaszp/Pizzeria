using AccountApi.Entities;
using AccountApi.Exceptions;
using AccountApi.Models;
using AccountApi.Services;
using DataAccess;
using Domain.Models;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using Pizzeria.Service.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pizzeria.Service.Services
{
    public class PizzeriaUserService : IPizzeriaUserService
    {
        private readonly IUserService userService;
        private readonly PizzeriaDbContext dbContext;

        public PizzeriaUserService(IUserService userService, IRoleService roleService, PizzeriaDbContext dbContext)
        {
            this.userService = userService;
            this.dbContext = dbContext;
        }

        public PizzeriaUser RegisterUser(CreateUserDto dto)
        {
            var serviceUser = userService.CreateUser(dto);
            var newPizzeriaUser = new PizzeriaUser()
            {
                ServiceUserId = serviceUser.Id,
            };
            dbContext.Add(newPizzeriaUser);
            dbContext.SaveChanges();

            return newPizzeriaUser;
        }
        public User GetAccountInfo(int pizzeriaUserId)
        {
            var pizzeriauser = dbContext
             .Users
             .FirstOrDefault(u => u.Id == pizzeriaUserId);

            if (pizzeriauser == null)
            {
                throw new NotFoundException("user not found");
            }
            else
            {
                return userService.GetById(pizzeriauser.ServiceUserId);
            }
        }
        public IEnumerable<Order> GetAllUserOrders(int UserId)
        {
            var pizzeriauser = dbContext
             .Users
             .FirstOrDefault(u => u.Id == UserId);

            if (pizzeriauser == null)
                throw new NotFoundException("user not found");

            if (pizzeriauser.Orders.Any())
            {
                return pizzeriauser.Orders.ToList();
            }
            else
            {
                throw new NotFoundException("this user have no orders");
            }
        }

        public IEnumerable<User> GetAllAccounts()
        {
            return userService.GetAll();

        }

        public void DeleteUser(int pizzeriaUserId)
        {
            var pizzeriauser = dbContext
                .Users
                .FirstOrDefault(u => u.Id == pizzeriaUserId);

            if (pizzeriauser is null)
                throw new NotFoundException("User not found");

            var accountID = pizzeriauser.ServiceUserId;

            dbContext.Users.Remove(pizzeriauser);
            userService.DeleteUser(accountID);
            dbContext.SaveChanges();
        }

        public void AssignRole(int roleId, int pizzeriaUserId)
        {
            var pizzeriauser = dbContext
               .Users
               .FirstOrDefault(u => u.Id == pizzeriaUserId);

            if (pizzeriauser is null)
                throw new NotFoundException("User not found");

            var accountID = pizzeriauser.ServiceUserId;
            userService.AssignRole(roleId, accountID);
        }

        public User GetAccountById(int pizzeriaUserId)
        {
            var pizzeriauser = dbContext
              .Users
              .FirstOrDefault(u => u.Id == pizzeriaUserId);

            if (pizzeriauser is null)
                throw new NotFoundException("User not found");

            var accountID = pizzeriauser.ServiceUserId;

            return userService.GetById(accountID);
        }

        public void GetJwtToken(LoginDto dto)
        {
            userService.GenerateJwt(dto);
        }
        public User GetUserLogin(LoginDto dto)
        {
            return userService.GetUserLogin(dto);
        }
    }
}
