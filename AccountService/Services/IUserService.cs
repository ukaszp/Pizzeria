using AccountApi.Entities;
using AccountApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AccountApi.Services
{
    public interface IUserService
    {
        User CreateUser(CreateUserDto dto);
        public IEnumerable<User> GetAllSearch(string search);
        public IEnumerable<User> GetAll();
        User GetById(int id);
        void DeleteUser(int id);
        public bool UpdateUser(int id, UpdateUserDto dto);
        public void AssignRole(int userId, int roleId);
        public string GenerateJwt(LoginDto dto);
        public User GetUserLogin(LoginDto dto);

    }
}