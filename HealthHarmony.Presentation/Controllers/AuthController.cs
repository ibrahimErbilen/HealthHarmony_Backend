using Microsoft.AspNetCore.Mvc;
using HealthHarmony.Entities.DTOs;
using HealthHarmony.Services.Contracts;
using HealthHarmony.Entities.DTOs.User;
using Microsoft.AspNetCore.Http;


namespace HealthHarmony.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IServiceManager _servieManager;

        public AuthController(IServiceManager servieManager)
        {
            _servieManager = servieManager;
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Register(UserCreateDto dto)
        {
            var result = _servieManager.AuthService.Register(dto);
            if (result == null) return BadRequest("Kayıt başarısız.");
            return Ok(result);
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult Login(UserLoginDto dto)
        {
            var result = _servieManager.AuthService.Login(dto);
            if (result == null) return Unauthorized();
            return Ok(result);
        }

        [HttpPost("refresh")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult Refresh([FromBody] Entities.DTOs.Token.RefreshTokenRequestDto dto)
        {
            var result = _servieManager.AuthService.RefreshToken(dto.RefreshToken);
            if (result == null)
                return Unauthorized();

            return Ok(result);
        }
    }
}
