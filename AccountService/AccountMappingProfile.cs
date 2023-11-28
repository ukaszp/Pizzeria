using AutoMapper;
using AccountApi.Entities;
using AccountApi.Models;

namespace AccountApi
{
    public class AccountMappingProfile:Profile
    {
        public AccountMappingProfile()
        {
            CreateMap<User, CreateUserDto>();
        }
    }
}
