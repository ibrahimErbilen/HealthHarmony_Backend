using HealthHarmony.Entities.DTOs.Food;
using HealthHarmony.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthHarmony.Repositories.Contracts
{
    public interface IFoodRepository
    {
        Task AddFood(Food food);
        Task<Food> SearchFoodByName(string name);
        Task AddDailyFoodEat(DailyFoodEat dailyFood);
        List<DailyFoodEat> GetTodayDailyFoodEatByUserId(Guid userId);


    }
}
