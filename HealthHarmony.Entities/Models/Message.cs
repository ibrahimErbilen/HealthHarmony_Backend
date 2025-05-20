using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthHarmony.Entities.Models
{
    public class Message
    {
        public int MessageId { get; set; }
        public Guid? SenderUserId { get; set; }
        public Guid? ReceiverUserId { get; set; }
        public Guid? SenderCoachId { get; set; }
        public string MessageContent { get; set; }
        public DateTime SentTime { get; set; }
        public bool IsRead { get; set; }
    }
}
