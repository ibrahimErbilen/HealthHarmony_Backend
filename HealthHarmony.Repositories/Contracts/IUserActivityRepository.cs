using HealthHarmony.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthHarmony.Repositories.Contracts
{
    public interface IUserActivityRepository
    {
        Task<IEnumerable<UserActivity>> GetAllUserActivitiesAsync();
        Task<UserActivity> GetUserActivityByIdAsync(Guid userActivityId);
        Task<IEnumerable<UserActivity>> GetUserActivitiesByUserIdAsync(Guid userId);
        Task<int> AddUserActivityAsync(UserActivity userActivity);
        Task<int> UpdateUserActivityAsync(UserActivity userActivity);
        Task<Guid> DeleteUserActivityAsync(Guid userActivityId);
        Task<int> GetUserPendingCaloriesBurnAsync(Guid userId);
        Task<bool> UpdateIsCompletedAsync(int userActivityId, bool isCompleted);
    }
}
