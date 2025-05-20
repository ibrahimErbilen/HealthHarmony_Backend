using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HealthHarmony.Entities.Models.Enums;

namespace HealthHarmony.Entities.Models
{
    public class UserFriend
    {
        public int FriendshipId { get; set; }
        public Guid UserId { get; set; }
        public Guid FriendId { get; set; }
        public DateTime StartDate { get; set; }
        public FriendshipStatus Status { get; set; }
    }
}
