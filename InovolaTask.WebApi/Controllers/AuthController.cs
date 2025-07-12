using InovolaTask.Application.Dto;
using InovolaTask.Application.Services.AuthServices;
using Microsoft.AspNetCore.Mvc;

namespace InovolaTask.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        #region Fields
        private readonly IAuthService _service;
        #endregion
        #region Constractor
        public AuthController(IAuthService service)
        {
            _service = service;
        }
        #endregion
        #region Actions
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto dto)
        {
            var result = await _service.Login(dto);
            return Ok(result);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationDto dto)
        {
            var result = await _service.Register(dto);
            return Ok(result);
        }
        #endregion
    }
}
