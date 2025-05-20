using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using HealthHarmony.Services.Contracts;
using HealthHarmony.Entities.DTOs;
using HealthHarmony.Entities.DTOs.Food;

namespace HealthHarmony.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FoodController : ControllerBase
    {
        private readonly IFoodService _foodService;
        private readonly IDailyDataService _dailyDataService;

        public FoodController(IFoodService foodService, IDailyDataService dailyDataService)
        {
            _foodService = foodService;
            _dailyDataService = dailyDataService;
        }

        // POST: api/food
        [HttpPost]
        public async Task<IActionResult> AddFood([FromBody] CreateFoodDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.FoodName) || dto.Calories < 0)
                return BadRequest("Geçersiz veri.");

            await _foodService.AddFood(dto);
            return Ok("Yemek başarıyla eklendi.");
        }

        // GET: api/food/{name}
        [HttpGet("{name}")]
        public async Task<IActionResult> GetFoodByName(string name)
        {
            var food = await _foodService.SearchFoodByName(name);

            if (food == null)
                return NotFound("Yemek bulunamadı.");

            return Ok(food);
        }

        [HttpPost("addDailyFood")]
        public async Task<IActionResult> AddDailyFoodEat([FromBody] CreateDailtFoodEatDto dailyFood)
        {
            if (dailyFood == null)
            {
                return BadRequest("Gelen veri boş.");
            }

            try
            {
                await _foodService.AddDailyFoodEat(dailyFood);
                Guid Userid = dailyFood.UserId;
                int calori = dailyFood.Calories;
                await _dailyDataService.UpdateCaloriesConsumedAsync(Userid,calori);

                return Ok("Yemek kaydı başarıyla eklendi.");
            }
            catch (Exception ex)
            {
                // Hata loglama yapılabilir
                return StatusCode(500, $"Sunucu hatası: {ex.Message}");
            }
        }

        [HttpGet("today/{userId}")]
        public ActionResult<List<DailyFoodDto>> GetTodayFoods(Guid userId)
        {
            if (userId == Guid.Empty)
                return BadRequest("Geçerli bir userId giriniz.");

            var foods = _foodService.GetTodayFoodsByUser(userId);
           

            if (foods == null || foods.Count == 0)
                return NotFound("Bugün için yemek verisi bulunamadı.");

            return Ok(foods);
        }


    }
}
