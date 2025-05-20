using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthHarmony.Entities.DTOs.User_Friend
{
    public record FriendRequestDTO
    {
        public int UserId { get; set; }
        public int FriendId { get; set; }
    }
}
