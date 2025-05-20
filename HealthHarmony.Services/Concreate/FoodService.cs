using HealthHarmony.Entities.DTOs;
using HealthHarmony.Entities.DTOs.Food;
using HealthHarmony.Entities.Models;
using HealthHarmony.Repositories;
using HealthHarmony.Repositories.Concreate;
using HealthHarmony.Repositories.Contracts;
using HealthHarmony.Services.Contracts;

public class FoodService : IFoodService
{
    private readonly IFoodRepository _foodRepository;

    public FoodService(IFoodRepository foodRepository)
    {
        _foodRepository = foodRepository;
    }

    public async Task AddDailyFoodEat(CreateDailtFoodEatDto dailyFood)
    {
        var daily = new DailyFoodEat
        {
            UserId = dailyFood.UserId,
            FoodName = dailyFood.FoodName,
            Calories = dailyFood.Calories,
            EatTime = DateTime.Now

        };

        await _foodRepository.AddDailyFoodEat(daily);
    }

    public async Task AddFood(CreateFoodDto dto)
    {
        var food = new Food
        {
            FoodName = dto.FoodName,
            Calories = dto.Calories
        };

        await _foodRepository.AddFood(food);
    }

    public List<DailyFoodDto> GetTodayFoodsByUser(Guid userId)
    {
        var entityList = _foodRepository.GetTodayDailyFoodEatByUserId(userId);

        

        var dtoList = entityList.Select(entity => new DailyFoodDto
        {
            DailyFoodEatID = entity.DailyFoodEatID,
            FoodName = entity.FoodName,
            Calories = entity.Calories,
            EatTime = entity.EatTime
        }).ToList();

        return dtoList;
    }

    public async Task<FoodDto> SearchFoodByName(string name)
    {
        var food = await _foodRepository.SearchFoodByName(name);
        if (food == null)
            return null;

        return new FoodDto
        {
            FoodId = food.FoodId,
            FoodName = food.FoodName,
            Calories = food.Calories
        };
    }
}
