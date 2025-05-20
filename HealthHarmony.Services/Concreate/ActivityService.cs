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
    public class ActivityService : IActivityService
    {
        private readonly IActivitiesRepository _activitiesRepository;

        public ActivityService(IActivitiesRepository activitiesRepository)
        {
            _activitiesRepository = activitiesRepository;
        }

        public async Task<IEnumerable<Activity>> GetAllActivitiesAsync()
        {
            return await _activitiesRepository.GetAllActivities();
        }

        public Task<int> GetEstimatedCaloriesBurnAsync(int activityId)
        {
            return _activitiesRepository.GetEstimatedCaloriesBurnAsync(activityId);
        }
    }
}
