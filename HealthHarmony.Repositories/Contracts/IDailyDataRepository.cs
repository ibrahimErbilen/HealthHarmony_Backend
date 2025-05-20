using HealthHarmony.Entities.DTOs.Daily;
using HealthHarmony.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthHarmony.Repositories.Contracts
{
    public interface IDailyDataRepository
    {
        void SaveOrUpdateDailyData(DailyData data);
        DailyData? GetTodayData(Guid userId);
        Task<List<DailyData>> GetLast6DaysCaloriesBurnedAsync(Guid userId);
        Task UpdateCaloriesConsumedAsync(Guid userId, int calori);
        Task UpdateCaloriesBurnedAsync(Guid userID, int calori);
        
    }
}
