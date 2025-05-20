using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthHarmony.Entities.DTOs.User
{
    public record UserDto
    {
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string ProfileImageUrl { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}

