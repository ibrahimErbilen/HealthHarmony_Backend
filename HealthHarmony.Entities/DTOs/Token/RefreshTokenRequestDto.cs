using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthHarmony.Entities.DTOs.Token
{
    public record RefreshTokenRequestDto
    {
        public string RefreshToken { get; set; }

    }
}
