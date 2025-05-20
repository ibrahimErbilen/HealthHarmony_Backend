using HealthHarmony.Entities.DTOs.Food;
using HealthHarmony.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthHarmony.Services.Contracts
{
    public interface IFoodService
    {
        Task AddFood(CreateFoodDto food);
        Task<FoodDto> SearchFoodByName(string name);
        Task AddDailyFoodEat(CreateDailtFoodEatDto dailyFood);
        List<DailyFoodDto> GetTodayFoodsByUser(Guid userId);
    }
}
