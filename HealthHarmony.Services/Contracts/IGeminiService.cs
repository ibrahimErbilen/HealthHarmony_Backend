using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthHarmony.Services.Contracts
{
    public interface IGeminiService
    {
        Task<string> AskGeminiAsync(string prompt);
    }

}
