using HealthHarmony.Entities.DTOs.Daily;
using HealthHarmony.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthHarmony.Services.Contracts
{
    public interface IDailyDataService
    {
        void AddOrUpdateDailyData(DailyDataDTO data);
        DailyDataDTO? GetTodayData(Guid userId);
        Task<List<DailyDataDTO>> GetLast6DaysCaloriesBurnedAsync(Guid userId);
        Task UpdateCaloriesConsumedAsync(Guid userId, int calori);
        Task UpdateCaloriesBurnedAsync(Guid userID, int calori);

    }
}
