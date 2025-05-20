using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthHarmony.Entities.DTOs.Coach_User
{
    public record CoachUserCreateDTO
    {
        public string InvitationCode { get; set; }
        public int UserId { get; set; }
    }
}
