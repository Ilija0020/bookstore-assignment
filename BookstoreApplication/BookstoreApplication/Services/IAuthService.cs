using BookstoreApplication.Services.DTOs;

namespace BookstoreApplication.Services
{
    public interface IAuthService
    {
        Task Login(LoginDto data);
        Task RegisterAsync(RegistrationDto data);
    }
}
