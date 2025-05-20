using HealthHarmony.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthHarmony.Repositories.Contracts
{
    public interface IActivitiesRepository
    {
        Task<IEnumerable<Activity>> GetAllActivities();
        Task<int> GetEstimatedCaloriesBurnAsync(int activityId);
    }
}
