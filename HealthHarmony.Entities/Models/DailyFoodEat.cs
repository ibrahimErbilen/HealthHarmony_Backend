using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthHarmony.Entities.Models
{
    public class DailyFoodEat
    {
        public int DailyFoodEatID { get; set; }
        public Guid UserId { get; set; }
        public string FoodName { get; set; }
        public int Calories { get; set; }
        public DateTime EatTime { get; set; }
    }
}
