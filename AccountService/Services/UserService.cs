using AutoMapper;
using Azure.Identity;
using AccountApi.Authentication;
using AccountApi.Entities;
using AccountApi.Exceptions;
using AccountApi.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace AccountApi.Services
{
    public class UserService : IUserService
    {
        private readonly AccountDbContext dbContext;
        private readonly ILogger<UserService> logger;
        private readonly IPasswordHasher passwordHasher;
        private readonly IMapper mapper;
        private readonly AuthenticationSettings authenticationSettings;
        private readonly IUserContextService userContextService;

        public UserService(AccountDbContext dbContext, ILogger<UserService> logger, IPasswordHasher passwordHasher, IMapper mapper, AuthenticationSettings authenticationSettings, IUserContextService userContextService)
        {
            this.dbContext = dbContext;
            this.logger = logger;
            this.passwordHasher = passwordHasher;
            this.mapper = mapper;
            this.authenticationSettings = authenticationSettings;
            this.userContextService = userContextService;
        }

        public User GetById(int id)
        {
            var user = dbContext
                .Users
                .FirstOrDefault(u => u.Id == id);

            return user == null ? throw new NotFoundException("User not found") : user;
        }

        public IEnumerable<User> GetAllSearch(string search)
        {

            var users = dbContext
                .Users
                .Where(r => r.LastName.ToLower().Contains(search.ToLower())
                    || r.Name.ToLower().Contains(search.ToLower()))
                .ToList();

            return users;
        }
        public IEnumerable<User> GetAll()
        {
            var users = dbContext
                .Users
                .ToList();

            return users;
        }

        public User CreateUser(CreateUserDto dto)
        {
            var passwordHash=passwordHasher.Hash(dto.Password);
            var newUser = new User()
            { 
                Name = dto.Name,
                LastName = dto.LastName,
                Email = dto.Email,
                ContactNumber = dto.ContactNumber,
                PasswordHash = passwordHash,
                Gender = dto.Gender,
                DateOfBirth = dto.DateOfBirth,
                RoleId = dto.RoleId
            };

            dbContext.Users.Add(newUser);
            dbContext.SaveChanges();
            return newUser;
        }

        public void DeleteUser(int id)
        {
            logger.LogWarning($"User with id: {id} DELETE action invoked");

            var user = dbContext
                .Users
                .FirstOrDefault(u => u.Id == id);

            if (user is null)
                throw new NotFoundException("User not found");

            dbContext .Users.Remove(user);
            dbContext.SaveChanges ();
        }

        public bool UpdateUser(int id, UpdateUserDto dto)
        {
            var userdb = dbContext
              .Users
              .FirstOrDefault(u => u.Id == id);

            if (userdb is null)
                throw new NotFoundException("User not found");

            userdb.Name = dto.Name;
            userdb.LastName = dto.LastName;
            userdb.Email = dto.Email;
            userdb.ContactNumber = dto.ContactNumber;
            userdb.Gender = dto.Gender;
            userdb.DateOfBirth = dto.DateOfBirth;

            dbContext.SaveChanges();

            return true;
        }

        public void AssignRole(int roleId, int userId) 
        { 
            var userdb = dbContext
                .Users
                .FirstOrDefault(u => u.Id == userId);

            var roledb = dbContext
               .Roles
               .FirstOrDefault(u => u.Id == roleId);

            if (userdb is null)
                throw new NotFoundException("user not found");
            if (roledb is null)
                throw new NotFoundException("Role not found");

            userdb.RoleId = roleId; 
            userdb.Role = roledb;
            dbContext.SaveChanges();
        }

        public string GenerateJwt(LoginDto dto)
        {
            var user = dbContext.Users
                .Include(u => u.Role)
                .FirstOrDefault(u => u.Email == dto.Email);

            if (user is null)
            {
                throw new ExceptionBadRequest("Invalid username or password");
            }

            var result = passwordHasher.Verify(user.PasswordHash, dto.Password);
            if (!result)
            {
                throw new ExceptionBadRequest("Invalid username or password");
            }

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{user.Name} {user.LastName}"),
                new Claim(ClaimTypes.Role, $"{user.Role.Name}"),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey.PadRight(256 / 8)));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(authenticationSettings.JwtExpireDays);

            var token = new JwtSecurityToken(authenticationSettings.JwtIssuer,
                authenticationSettings.JwtIssuer,
                claims,
                DateTime.UtcNow,
                expires: expires,
                signingCredentials: cred);

            var tokenHandler = new JwtSecurityTokenHandler();
            tokenHandler.WriteToken(token);
            return tokenHandler.WriteToken(token);
        }
        public User GetUserLogin(LoginDto dto)
        {
            var user = dbContext.Users
                .Include(u => u.Role)
                .FirstOrDefault(u => u.Email == dto.Email);

            return user;
        }
    }
}
