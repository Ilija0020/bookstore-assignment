using BookstoreApplication.Services.DTOs;
using System.Security.Claims;

namespace BookstoreApplication.Services
{
    public interface IAuthService
    {
        Task<string> Login(LoginDto data);
        Task RegisterAsync(RegistrationDto data);
        Task<ProfileDto> GetProfile(ClaimsPrincipal userPrincipal);
    }
}
