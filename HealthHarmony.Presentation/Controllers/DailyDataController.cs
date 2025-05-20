using HealthHarmony.Entities.DTOs.Daily;
using HealthHarmony.Entities.Models;
using HealthHarmony.Services.Concreate;
using HealthHarmony.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HealthHarmony.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DailyDataController : ControllerBase
    {
        private readonly IDailyDataService _service;

        public DailyDataController(IDailyDataService service)
        {
            _service = service;
        }

        [HttpPost("{userId}")]
        public IActionResult SaveDailyData(Guid userId, [FromBody] DailyDataDTO model)
        {
            model.UserId = userId;
            _service.AddOrUpdateDailyData(model);
            return Ok("Daily data recorded successfully.");
        }

        [HttpGet("today")]
        public IActionResult GetTodayData([FromQuery] Guid userId )
        {
            try
            {
                var dailyData = _service.GetTodayData(userId);

                if (dailyData == null)
                {
                    // Eğer veri bulunamazsa, yeni bir veri kaydediyoruz
                    var newDailyData = new DailyDataDTO
                    {
                        UserId = userId,
                        StepCount = 0,  // Varsayılan verilerle doldurulabilir
                        CaloriesBurned = 0,
                        CaloriesConsumed = 0,
                        Date = DateTime.Now.Date
                    };

                    // Burada, veriyi kaydetmek için servis metodunu çağırıyoruz
                    _service.AddOrUpdateDailyData(newDailyData);

                    // Kaydettiğimiz veriyi geri döndürüyoruz
                    return Ok(newDailyData);
                }

                return Ok(dailyData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Bir hata oluştu", details = ex.Message });
            }
        }

        [HttpGet("{userId}/last6days")]
        public async Task<IActionResult> GetLast6DaysCaloriesBurnedAsync(Guid userId)
        {
            try
            {
                var result = await _service.GetLast6DaysCaloriesBurnedAsync(userId);
                
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }





    }
}
