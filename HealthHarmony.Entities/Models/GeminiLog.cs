using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthHarmony.Entities.Models
{
    public class GeminiLog
    {
        public string UserId { get; set; }
        public string Prompt { get; set; }
        public string Response { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
