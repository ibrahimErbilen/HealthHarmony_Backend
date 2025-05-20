using HealthHarmony.Entities.DTOs.User;
using HealthHarmony.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthHarmony.Services.Contracts
{
    public interface IUserService
    {
        UserDto GetByEmail(string email);
        UserDto GetById(Guid id);
        void Add(UserCreateDto user); // DTO ile yeni kullanıcı alımı
        bool SaveChanges();
        void Update(UserUpdateDTO user);
        UserDto GetByRefreshToken(string refreshToken);
    }
}
