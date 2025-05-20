using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HealthHarmony.Entities.Models;
using HealthHarmony.Entities.DTOs;
using HealthHarmony.Repositories.Contracts;
using Microsoft.IdentityModel.Tokens;
using HealthHarmony.Services.Contracts;
using Microsoft.Extensions.Configuration;
using HealthHarmony.Entities.DTOs.User;
using System.Security.Cryptography;
using HealthHarmony.Entities.DTOs.Token;
using HealthHarmony.Repositories;
using System.Diagnostics;

namespace HealthHarmony.Service
{
    public class AuthService : IAuthService
    {
        private readonly IRepositoryManager _manager;
        private readonly ITokenService _tokenService;

        // TokenService'i enjekte ettik
        public AuthService(IRepositoryManager manager, ITokenService tokenService)
        { // TokenService'i kullanmak için
            _manager = manager;
            _tokenService = tokenService;
        }

        public UserRegisterDto Register(UserCreateDto dto)
        {
            var stock = Stopwatch.StartNew();

            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.PasswordHash),
                ProfileImageUrl = dto.ProfileImageUrl,
                RegistrationDate = DateTime.UtcNow,
                RefreshToken = _tokenService.GenerateRefreshToken(), // TokenService üzerinden RefreshToken
                RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(30) // RefreshToken süresi 30 gün
            };

            _manager.User.Add(user);
            _manager.User.SaveChanges();

            var token = _tokenService.CreateToken(new UserDto
            {
                UserId = user.UserId,
                Email = user.Email
            });

            stock.Stop();



            return new UserRegisterDto
            {
                UserId = user.UserId,
                Username = user.Username,
                Email = user.Email,
                Token = token.AccessToken, // TokenService üzerinden JWT token oluşturuluyor
                RefreshToken = user.RefreshToken
            };
        }

        public UserRegisterDto Login(UserLoginDto dto)
        {
            var user = _manager.User.GetByEmail(dto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                return null;

            user.RefreshToken = _tokenService.GenerateRefreshToken(); // TokenService üzerinden RefreshToken
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(30);
            _manager.User.Update(user);
            _manager.User.SaveChanges();

            var token = _tokenService.CreateToken(new UserDto
            {
                UserId = user.UserId,
                Email = user.Email
            });

            return new UserRegisterDto
            {
                UserId = user.UserId,
                Username = user.Username,
                Email = user.Email,
                Token = token.AccessToken, 
                RefreshToken = user.RefreshToken
            };
        }
        public TokenDto RefreshToken(string refreshToken)
        {
            var user = _manager.User.GetByRefreshToken(refreshToken);
            if (user == null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
                return null;

            var newRefreshToken = _tokenService.GenerateRefreshToken();
            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(30);
            _manager.User.Update(user);

            var userDto = new UserDto
            {
                UserId = user.UserId,
                Email = user.Email
            };

            var token = _tokenService.CreateToken(userDto);
            token.RefreshToken = newRefreshToken;
            token.RefreshTokenExpiration = (DateTime)user.RefreshTokenExpiryTime;

            return token;
        }
    }
}
