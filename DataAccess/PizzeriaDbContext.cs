using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class PizzeriaDbContext:DbContext
    {
        readonly string _connectrionString =
            "Server = (localdb)\\MSSQLLocalDB; Database=PizzeriaDb;Trusted_Connection=True";
        public PizzeriaDbContext(DbContextOptions opt):base(opt)
        {
            
        }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<PizzeriaUser> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectrionString);
        }
    }
}
