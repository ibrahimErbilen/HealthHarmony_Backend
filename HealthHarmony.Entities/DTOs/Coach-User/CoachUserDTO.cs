using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthHarmony.Entities.DTOs.Coach_User
{
    public record CoachUserDTO
    {
        public int CoachUserId { get; set; }
        public int CoachId { get; set; }
        public int UserId { get; set; }
        public string CoachName { get; set; }
        public string Username { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; }
    }
}
