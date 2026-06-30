using AutoMapper;
using BookstoreApplication.Models;
using BookstoreApplication.Services.DTOs;
using BookstoreApplication.Services.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookstoreApplication.Services
{
    public class AuthService : IAuthService
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<ApplicationUser> userManager, IMapper mapper, IConfiguration configuration)
        {
            _userManager = userManager;
            _mapper = mapper;
            _configuration = configuration;
        }
        public async Task<string> Login(LoginDto data)
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

            var token = await GenerateJwt(user);
            return token;
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

        public async Task<ProfileDto> GetProfile(ClaimsPrincipal userPrincipal)
        {
            // Preuzimanje korisničkog imena iz tokena
            var username = userPrincipal.FindFirstValue("username");

            if (username == null)
            {
                throw new BadRequestException("Token is invalid");
            }

            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                throw new BadRequestException("User with provided username does not exist");
            }

            return _mapper.Map<ProfileDto>(user);
        }


        private async Task<string> GenerateJwt(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim("username", user.UserName ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: creds
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
