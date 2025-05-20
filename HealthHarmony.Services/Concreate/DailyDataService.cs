using HealthHarmony.Entities.DTOs.Daily;
using HealthHarmony.Entities.DTOs.User_Activity;
using HealthHarmony.Entities.Models;
using HealthHarmony.Repositories.Contracts;
using HealthHarmony.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthHarmony.Services.Concreate
{
    public class DailyDataService : IDailyDataService
{
    private readonly IDailyDataRepository _repository;

    public DailyDataService(IDailyDataRepository repository)
    {
        _repository = repository;
    }

        public void AddOrUpdateDailyData(DailyDataDTO data)
    {
        data.Date = DateTime.Now.Date; // Her gün için sabit tarih
        var entity = MapToEntity(data);
        _repository.SaveOrUpdateDailyData(entity);
    }
        public async Task<List<DailyDataDTO>> GetLast6DaysCaloriesBurnedAsync(Guid userId)
        {
            var entities = await _repository.GetLast6DaysCaloriesBurnedAsync(userId);
            return entities.Select(MapToDto).ToList();
        }

        public DailyDataDTO? GetTodayData(Guid userId)
    {
        var entity = _repository.GetTodayData(userId);
        return entity != null ? MapToDto(entity) : null;
    }

        public Task UpdateCaloriesBurnedAsync(Guid userID, int calori)
        {
            return _repository.UpdateCaloriesBurnedAsync(userID, calori);
        }

        public Task UpdateCaloriesConsumedAsync(Guid userId, int calori)
        {
            return _repository.UpdateCaloriesConsumedAsync(userId,calori);
        }

        private DailyDataDTO MapToDto(DailyData entity)
    {
        return new DailyDataDTO
        {
            DailyDataId = entity.DailyDataId,
            UserId = entity.UserId,
            Date = entity.Date,
            StepCount = entity.StepCount,
            CaloriesBurned = entity.CaloriesBurned,
            CaloriesConsumed = entity.CaloriesConsumed
        };
    }

    private DailyData MapToEntity(DailyDataDTO dto)
    {
        return new DailyData
        {
            DailyDataId = dto.DailyDataId,
            UserId = dto.UserId,
            Date = dto.Date,
            StepCount = dto.StepCount,
            CaloriesBurned = dto.CaloriesBurned,
            CaloriesConsumed = dto.CaloriesConsumed
        };
    }
}

}
