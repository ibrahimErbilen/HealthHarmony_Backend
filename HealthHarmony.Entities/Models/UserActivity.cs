using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthHarmony.Entities.Models
{
    public class UserActivity
    {
        public int UserActivityId { get; set; }
        public Guid UserId { get; set; } //GUID
        public int ActivityId { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public bool IsCompleted { get; set; }
        public string ActivityName { get; set; }
        public string Description { get; set; }
    }
}
