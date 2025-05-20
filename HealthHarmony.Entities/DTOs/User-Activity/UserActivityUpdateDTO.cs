using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthHarmony.Entities.DTOs.User_Activity
{
    public record UserActivityUpdateDTO
    {
        public bool IsCompleted { get; set; }
    }
}
