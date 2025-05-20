using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthHarmony.Entities.DTOs.Activity
{
    public record ActivityCreateDTO
    {
        public string ActivityName { get; set; }
        public string Description { get; set; }
        public int EstimatedCaloriesBurn { get; set; }
        public int DifficultyLevel { get; set; }
        public string ImageUrl { get; set; }
        public string VideoUrl { get; set; }
    }
}
