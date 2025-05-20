using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthHarmony.Entities.DTOs.User_Activity
{
    public record UserActivityCreateDTO
    {
        public int UserId { get; set; }
        public int ActivityId { get; set; }
    }
}
