using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using HealthHarmony.Service;
using HealthHarmony.Entities.DTOs;
using HealthHarmony.Services.Contracts;
using HealthHarmony.Entities.DTOs.User_Activity;
using HealthHarmony.Services.Concreate;
using HealthHarmony.Entities.Models;

namespace HealthHarmonyPresentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserActivityController : ControllerBase
    {
        private readonly IUserActivityService _userActivityService;
        private readonly IActivityService _activityService;
        private readonly IDailyDataService _dailyDataService;

        public UserActivityController(IUserActivityService userActivityService, IActivityService activityService, IDailyDataService dailyDataService)
        {
            _userActivityService = userActivityService;
            _activityService = activityService;
            _dailyDataService = dailyDataService;
        }

        // Tüm aktiviteleri almak için
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _userActivityService.GetAllAsync();
            if (result == null || !result.Any())
                return NotFound("No user activities found.");

            return Ok(result);
        }

        // ID'ye göre tek bir aktiviteyi almak için
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _userActivityService.GetByIdAsync(id);
            if (result == null)
                return NotFound($"User activity with ID {id} not found.");

            return Ok(result);
        }

        // Kullanıcıya göre aktiviteleri almak için
        [HttpGet("by-user/{userId}")]
        public async Task<IActionResult> GetByUserId(Guid userId)
        {
            var result = await _userActivityService.GetByUserIdAsync(userId);
            if (result == null || !result.Any())
                return NotFound($"No activities found for user with ID {userId}.");

            return Ok(result);
        }

        // Yeni aktivite oluşturmak için
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserActivityDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                // Yeni aktiviteyi ekliyoruz ve yeni ID'yi döndürüyoruz
                var newId = await _userActivityService.AddAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = newId }, dto);
            }
            catch (Exception ex)
            {
                // Eğer hata oluşursa, uygun bir mesaj dönüyoruz
                return StatusCode(500, $"An error occurred while creating the activity: {ex.Message}");
            }
        }

        // Var olan bir aktiviteyi güncellemek için
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UserActivityDto dto)
        {
            if (id != dto.UserId)  // Burada DTO'nun UserActivityId'yi kullanmalısınız
                return BadRequest("ID mismatch");

            var existing = await _userActivityService.GetByIdAsync(id);
            if (existing == null)
                return NotFound($"User activity with ID {id} not found.");

            try
            {
                dto.IsCompleted = true;
                await _userActivityService.UpdateAsync(dto);
                return NoContent();  // Güncelleme başarılı, boş içerik döndürüyoruz
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while updating the activity: {ex.Message}");
            }
        }

        // Bir aktiviteyi silmek için
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var existing = await _userActivityService.GetByIdAsync(id);
            if (existing == null)
                return NotFound($"User activity with ID {id} not found.");

            try
            {
                await _userActivityService.DeleteAsync(id);
                return NoContent();  // Silme işlemi başarılı, boş içerik döndürüyoruz
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while deleting the activity: {ex.Message}");
            }
        }

        [HttpPut("UpdateActivity/{userActivityId}")]
        public async Task<IActionResult> UpdateUserActivity(int userActivityId, UserActivity dto)
        {
            // Yeni 'IsCompleted' değerini güncelle
            var result = await _userActivityService.UpdateIsCompletedAsync(userActivityId, dto.IsCompleted);
            int activityid = dto.ActivityId;
            var result2 = await _activityService.GetEstimatedCaloriesBurnAsync(activityid);
            Guid UserId = dto.UserId;
            int calori = result2;
            await _dailyDataService.UpdateCaloriesBurnedAsync(UserId ,calori);

            if (result)
            {
                return Ok(new { message = "User Activity updated successfully." });
            }
            else
            {
                return BadRequest("An error occurred while updating the activity.");
            }
        }

        [HttpGet("pending-calories-burn/{userId}")]
        public async Task<ActionResult<int>> GetPendingCaloriesBurned(Guid userId)
        {
            try
            {

                return Ok(await _userActivityService.GetUserPendingCaloriesBurnAsync(userId));
            }
            catch (Exception ex)
            {
                // Log the exception (logging implementation not shown)
                return StatusCode(500, "An error occurred while retrieving pending calories");
            }
        }
    }
}
