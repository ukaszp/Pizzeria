using AccountApi.Models;
using AccountApi.Services;
using DataAccess;
using Domain.Models;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pizzeria.Service.Services
{
    internal class PizzeriaUserService
    {
        private readonly IUserService userService;
        private readonly IRoleService roleService;
        private readonly PizzeriaDbContext dbContext;

        public PizzeriaUserService(IUserService userService, IRoleService roleService, PizzeriaDbContext dbContext)
        {
            this.userService = userService;
            this.roleService = roleService;
            this.dbContext = dbContext;
        }

        public PizzeriaUser RegisterUser(CreateUserDto dto)
        {
            var serviceUser = userService.CreateUser(dto);
            var newPizzeriaUser = new PizzeriaUser()
            {
                ServiceUser = serviceUser,
                ServiceUserId = serviceUser.Id,
            };
            dbContext.Add(newPizzeriaUser);
            dbContext.SaveChanges();
            return newPizzeriaUser;
        }
    }
}
