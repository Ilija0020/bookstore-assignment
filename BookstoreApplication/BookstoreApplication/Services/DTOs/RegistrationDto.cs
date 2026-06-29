namespace BookstoreApplication.Services.DTOs
{
    public class RegistrationDto
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string Username { get; set; }
        public required string Name { get; set; }
        public required string Surname { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
