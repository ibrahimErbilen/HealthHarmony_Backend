using HealthHarmony.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthHarmony.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GeminiController : ControllerBase
    {
        private readonly IGeminiService _geminiService;

        public GeminiController(IGeminiService geminiService)
        {
            _geminiService = geminiService;
        }

        [HttpGet("ask")]
        public async Task<IActionResult> Ask([FromQuery] string prompt)
        {
            var result = await _geminiService.AskGeminiAsync(prompt);
            return Ok(result);
        }
    }
}
