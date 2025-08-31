using Microsoft.AspNetCore.Mvc;
using Scholarship_Plaatform_Backend.Models;
using Scholarship_Plaatform_Backend.Services;

namespace Scholarship_Plaatform_Backend.Controllers
{
    [ApiController]
    [Route("api/")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthenticationController(IAuthService authService)
        {
            this._authService = authService;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            try
            {
                var (status, message) = await _authService.Login(model);
                if (status == 0)
                {
                    return BadRequest(message);
                }
                return Ok(
                    new
                    {
                        Status = "Success",
                        token = message
                    }
                );
            }
            catch (Exception ex)
            {
                return StatusCode(
                        StatusCodes.Status500InternalServerError,
                        new { Status = "Error", Message = ex.Message }
                    );
            }
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(User model)
        {
            try
            {
                var (status, message) = await _authService.Registration(model, model.UserRole);
                if (status == 0)
                {
                    return BadRequest(message);
                }
                return Ok(new { msg = message });
            }
            catch (Exception ex)
            {
                return StatusCode(
                        StatusCodes.Status500InternalServerError,
                        new { Status = "Error", Message = ex.Message }
                    );
            }
        }

    }
}
