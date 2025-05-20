using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthHarmony.Entities.DTOs.Food
{
    public record CreateFoodDto
    {
        public string FoodName { get; set; }
        public int Calories { get; set; }
    }
}
