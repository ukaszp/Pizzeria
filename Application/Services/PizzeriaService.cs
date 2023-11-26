using AccountApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pizzeria.Service.Services
{
    internal class PizzeriaService
    {
        private readonly IUserService userService;
        private readonly IRoleService roleService;

        public PizzeriaService(IUserService userService, IRoleService roleService)
        {
            this.userService = userService;
            this.roleService = roleService;
        }
    }
}
