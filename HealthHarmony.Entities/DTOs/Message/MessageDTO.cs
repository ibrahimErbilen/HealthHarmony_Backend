using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthHarmony.Entities.DTOs.Message
{
    public record MessageDTO
    {
        public int MessageId { get; set; }
        public Guid? SenderUserId { get; set; }
        public string SenderName { get; set; }
        public Guid? ReceiverUserId { get; set; }
        public string ReceiverName { get; set; }
        public int? SenderCoachId { get; set; }
        public string SenderCoachName { get; set; }
        public string MessageContent { get; set; }
        public DateTime SentTime { get; set; }
        public bool IsRead { get; set; }
    }
}
