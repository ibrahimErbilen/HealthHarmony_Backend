using HealthHarmony.Entities.DTOs.User_Activity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthHarmony.Services.Contracts
{
    public interface IUserActivityService
    {
        Task<IEnumerable<UserActivityDto>> GetAllAsync();
        Task<UserActivityDto> GetByIdAsync(Guid id);
        Task<IEnumerable<UserActivityDto>> GetByUserIdAsync(Guid userId);
        Task<int> AddAsync(UserActivityDto dto);
        Task<int> UpdateAsync(UserActivityDto dto);
        Task<Guid> DeleteAsync(Guid id);
        Task GetAllActivitiesAsync();
        Task<int> GetUserPendingCaloriesBurnAsync(Guid userId);
        Task<bool> UpdateIsCompletedAsync(int userActivityId, bool isCompleted);
    }
}
