using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace AccountApi.Entities
{
    public class AccountDbContext:DbContext
    {
        readonly string _connectrionString=
            "Server = (localdb)\\MSSQLLocalDB; Database=PizzeriaAccountDb;Trusted_Connection=True";


        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
             .Property(u => u.Name)
             .IsRequired()
             .HasMaxLength(25);

            modelBuilder.Entity<User>()
             .Property(u => u.LastName)
             .IsRequired()
             .HasMaxLength(25);

            modelBuilder.Entity<User>()
             .Property(u => u.Email)
             .IsRequired()
             .HasMaxLength(25);

            modelBuilder.Entity<Role>()
             .Property(u => u.Name)
             .IsRequired()
             .HasMaxLength(25);

            modelBuilder.Entity<Role>()
             .Property(r => r.Id)
             .ValueGeneratedNever();

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectrionString);
        }
    }
}
