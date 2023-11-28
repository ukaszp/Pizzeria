using System.ComponentModel.DataAnnotations;

namespace AccountApi.Entities
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(25)]
        public string Name { get; set; }
        [Required]
        [MaxLength(25)]
        public string LastName { get; set; }
        [Required]
        [MaxLength(25)]
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        [Phone]
        public string ContactNumber { get; set; }
        public bool Gender { get; set; }
        public DateTime WhenJoined { get; set; }= DateTime.UtcNow;
        public DateTime? DateOfBirth { get; set; }
        public int RoleId { get; set; } = 2;
        public Role Role { get; set; }
    }
}
