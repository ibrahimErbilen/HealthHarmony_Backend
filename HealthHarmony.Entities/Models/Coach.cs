using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HealthHarmony.Entities.Models.Enums;

namespace HealthHarmony.Entities.Models
{
    public class Coach
    {
        public Guid CoachId { get; set; }
        public string CoachName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string InvitationCode { get; set; }
        public string ProfileImageUrl { get; set; }
        public CoachType CoachType { get; set; }
        public string Specialization { get; set; }
        public string Experience { get; set; }
    }

}
