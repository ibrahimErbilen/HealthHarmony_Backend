using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HealthHarmony.Entities.Models.Enums;

namespace HealthHarmony.Entities.DTOs.Coach
{
    public record CoachDTO
    {
        public Guid CoachId { get; set; }
        public string CoachName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string InvitationCode { get; set; }
        public string ProfileImageUrl { get; set; }
        public int CoachType { get; set; }
        public string Specialization { get; set; }
        public string Experience { get; set; }
    }
}
