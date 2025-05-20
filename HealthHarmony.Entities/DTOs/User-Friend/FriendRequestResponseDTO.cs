using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HealthHarmony.Entities.Models.Enums;

namespace HealthHarmony.Entities.DTOs.User_Friend
{
    public record FriendRequestResponseDTO
    {
        public int FriendshipId { get; set; }
        public FriendshipStatus Status { get; set; }
    }
}
