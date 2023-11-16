using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions
{
    public interface IAddressRepository
    {
        Task<Address> CreateAddress(Address address);
        Task DeleteAddress(int addressId);
        Task GetAddressById(int addressId);

    }
}
