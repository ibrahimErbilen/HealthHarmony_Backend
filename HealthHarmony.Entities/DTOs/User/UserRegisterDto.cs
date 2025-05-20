using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthHarmony.Entities.DTOs.User
{
    public record UserRegisterDto
    {
        public Guid UserId { get; set; } 
        public string Username { get; set; }
        public string Email { get; set; }
        public string Token { get; set; } // JWT token
        public string RefreshToken { get; set; }

    }
}
