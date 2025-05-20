using HealthHarmony.Entities.Models;
using HealthHarmony.Repositories.Contracts;
using HealthHarmony.Entities.DTOs.User;
using HealthHarmony.Services.Contracts;

namespace HealthHarmony.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public UserDto GetByEmail(string email)
        {
            var user = _userRepository.GetByEmail(email);
            return user != null ? MapToDto(user) : null;
        }

        public UserDto GetById(Guid id)
        {
            var user = _userRepository.GetById(id);
            return user != null ? MapToDto(user) : null;
        }

        public void Add(UserCreateDto dto)
        {
            var user = new User
            {
                UserId = Guid.NewGuid(),
                Username = dto.Username,
                Email = dto.Email,
                PasswordHash = dto.PasswordHash,
                ProfileImageUrl = dto.ProfileImageUrl,
                RegistrationDate = dto.RegistrationDate,
                RefreshToken = dto.RefreshToken,
                RefreshTokenExpiryTime = dto.RefreshTokenExpiryTime
            };

            _userRepository.Add(user);
        }

        public bool SaveChanges()
        {
            return _userRepository.SaveChanges();
        }

        public void Update(UserUpdateDTO dto)
        {
            var user = new User
            {
                UserId = dto.UserId,
                Username = dto.Username,
                Email = dto.Email,
                PasswordHash = dto.PasswordHash,
                ProfileImageUrl = dto.ProfileImageUrl,
                RegistrationDate = dto.RegistrationDate,
                RefreshToken = dto.RefreshToken,
                RefreshTokenExpiryTime = dto.RefreshTokenExpiryTime
            };

            _userRepository.Update(user);
        }

        public UserDto GetByRefreshToken(string refreshToken)
        {
            var user = _userRepository.GetByRefreshToken(refreshToken);
            return user != null ? MapToDto(user) : null;
        }

        private UserDto MapToDto(User user)
        {
            return new UserDto
            {
                UserId = user.UserId,
                Username = user.Username,
                Email = user.Email,
                ProfileImageUrl = user.ProfileImageUrl,
                RegistrationDate = user.RegistrationDate
            };
        }
    }
}
