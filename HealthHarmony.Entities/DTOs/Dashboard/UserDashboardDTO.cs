using HealthHarmony.Entities.DTOs.Coach_User;
using HealthHarmony.Entities.DTOs.Daily;
using HealthHarmony.Entities.DTOs.User;
using HealthHarmony.Entities.DTOs.User_Activity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthHarmony.Entities.DTOs.Dashboard
{
    public record UserDashboardDTO
    {
        public UserDto User { get; set; }
        public DailyDataDTO TodayData { get; set; }
        public List<UserActivityDto> TodayActivities { get; set; }
        public List<CoachUserDTO> MyCoaches { get; set; }
        public int UnreadMessagesCount { get; set; }
    }
}
