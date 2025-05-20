using Microsoft.AspNetCore.Mvc;
using HealthHarmony.Services.Contracts;
using HealthHarmony.Entities.DTOs.User;

namespace HealthHarmony.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("by-id/{id}")]
        public ActionResult<UserDto> GetById(Guid id)
        {
            var user = _userService.GetById(id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpGet("by-email")]
        public ActionResult<UserDto> GetByEmail([FromQuery] string email)
        {
            var user = _userService.GetByEmail(email);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpGet("by-refresh-token")]
        public ActionResult<UserDto> GetByRefreshToken([FromQuery] string token)
        {
            var user = _userService.GetByRefreshToken(token);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpPost]
        public IActionResult Create([FromBody] UserCreateDto dto)
        {
            _userService.Add(dto);
            var saved = _userService.SaveChanges();

            if (!saved)
                return BadRequest("User could not be created.");

            return Ok("User created successfully.");
        }

        [HttpPut]
        public IActionResult Update([FromBody] UserUpdateDTO dto)
        {
            _userService.Update(dto);
            _userService.SaveChanges();
            return Ok("User updated successfully.");
        }
    }
}
