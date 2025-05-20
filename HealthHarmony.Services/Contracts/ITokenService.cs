using HealthHarmony.Entities.DTOs.Token;
using HealthHarmony.Entities.DTOs.User;
using HealthHarmony.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthHarmony.Services.Contracts
{
    public interface ITokenService
    {
        TokenDto CreateToken(UserDto user);
        string GenerateRefreshToken();
    }
}
