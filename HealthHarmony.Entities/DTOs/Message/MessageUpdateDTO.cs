using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthHarmony.Entities.DTOs.Message
{
    public record MessageUpdateDTO
    {
        public bool IsRead { get; set; }
    }
}
