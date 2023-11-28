using AccountApi.Entities;
using AccountApi.Models;
using Domain.Models;

namespace Pizzeria.Service.Abstractions
{
    internal interface IPizzeriaUserService
    {
        void AssignRole(int roleId, int pizzeriaUserId);
        void DeleteUser(int pizzeriaUserId);
        User GetAccountById(int pizzeriaUserId);
        User GetAccountInfo(int pizzeriaUserId);
        IEnumerable<User> GetAllAccounts();
        IEnumerable<Order> GetAllUserOrders(int UserId);
        void GetJwtToken(LoginDto dto);
        User GetUserLogin(LoginDto dto);
        PizzeriaUser RegisterUser(CreateUserDto dto);
    }
}