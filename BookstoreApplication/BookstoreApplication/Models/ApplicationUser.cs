using Microsoft.AspNetCore.Identity;

namespace BookstoreApplication.Models
{
    public class ApplicationUser : IdentityUser
    {
        public required string Name { get; set; }
        public required string Surname { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
