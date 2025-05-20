using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HealthHarmony.Entities.Models.Enums;

namespace HealthHarmony.Entities.DTOs.Coach
{
    public record CoachUpdateDTO
    {
        public string CoachName { get; set; }
        public string Email { get; set; }
        public string ProfileImageUrl { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public CoachType CoachType { get; set; }
        public string Specialization { get; set; }
        public string Experience { get; set; }
    }
}
