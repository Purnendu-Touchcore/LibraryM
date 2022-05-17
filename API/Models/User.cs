
namespace API.Models
{
    // This class is used to represent a user in the database.
    public class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public int Role { get; set; } //foreign key for Roles table
        public byte[]? PasswordSalt { get; set; }
        public byte[]? PasswordHash { get; set; }
    }
}
