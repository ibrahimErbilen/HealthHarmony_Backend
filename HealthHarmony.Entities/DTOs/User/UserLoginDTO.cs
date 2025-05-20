using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthHarmony.Entities.DTOs.User
{
    public record UserLoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
