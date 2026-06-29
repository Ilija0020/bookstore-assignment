using AutoMapper;
using BookstoreApplication.Models;
using BookstoreApplication.Services.DTOs;
using BookstoreApplication.Services.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace BookstoreApplication.Services
{
    public class AuthService : IAuthService
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public AuthService(UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }
        public async Task Login(LoginDto data)
        {
            var user = await _userManager.FindByNameAsync(data.Username);
            if (user == null)
            {
                throw new BadRequestException("Invalid credentials.");
            }

            var passwordMatch = await _userManager.CheckPasswordAsync(user, data.Password);
            if (!passwordMatch)
            {
                throw new BadRequestException("Invalid credentials. ");
            }
        }

        public async Task RegisterAsync(RegistrationDto data)
        {
            var user = _mapper.Map<ApplicationUser>(data);
            var result = await _userManager.CreateAsync(user, data.Password);

            if(!result.Succeeded)
            {
                string errorMessage = string.Join(";", result.Errors.Select(e => e.Description));
                throw new BadRequestException(errorMessage);
            }
        }
    }
}
