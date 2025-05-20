using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthHarmony.Entities.DTOs.Daily
{
    public record DailyDataUpdateDTO
    {
        public int StepCount { get; set; }
        public int CaloriesBurned { get; set; }
        public int CaloriesConsumed { get; set; }
    }
}
