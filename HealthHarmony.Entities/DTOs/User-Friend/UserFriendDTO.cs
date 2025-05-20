using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HealthHarmony.Entities.Models.Enums;

namespace HealthHarmony.Entities.DTOs.User_Friend
{
    public record UserFriendDTO
    {
        public int FriendshipId { get; set; }
        public int UserId { get; set; }
        public int FriendId { get; set; }
        public string FriendUsername { get; set; }
        public string FriendProfileImageUrl { get; set; }
        public DateTime StartDate { get; set; }
        public FriendshipStatus Status { get; set; }
    }
}
