using HealthHarmony.Entities.DTOs.Token;
using HealthHarmony.Entities.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthHarmony.Services.Contracts
{
    public interface IAuthService
    {
        UserRegisterDto Register(UserCreateDto dto);
        UserRegisterDto Login(UserLoginDto dto);
        TokenDto RefreshToken(string refreshToken);
    }
}
