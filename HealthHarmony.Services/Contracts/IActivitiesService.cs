using HealthHarmony.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthHarmony.Services.Contracts
{
    public interface IActivityService
    {
        Task<IEnumerable<Activity>> GetAllActivitiesAsync();
        Task<int> GetEstimatedCaloriesBurnAsync(int activityId);
    }
}
