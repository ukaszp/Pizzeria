using AccountApi.Entities;

namespace AccountApi
{
    public class AccountSeeder
    {
        private readonly AccountDbContext _db;
        public AccountSeeder(AccountDbContext db)
        {
                _db = db;
        }
        public void Seed()
        {
            if(_db.Database.CanConnect())
            {
                if(!_db.Roles.Any())
                {
                    var roles = GetRoles();
                    _db.Roles.AddRange(roles);
                    _db.SaveChanges();
                }
            }
        }
        private IEnumerable<Role> GetRoles()
        {
            var roles = new List<Role>()
            {
                new Role()
                {
                    Id = 1,
                    Name="Admin",
                    Description="admin"
                },
                new Role()
                {
                    Id = 2,
                    Name="User",
                    Description = "user"
                }
            };
            return roles;
        }
    }
}
