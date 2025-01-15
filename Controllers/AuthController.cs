using EraZor.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Erazor.DTOs;

namespace WebKlient.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
            {
                return BadRequest(new { message = "Email and password are required." });
            }

            var user = await _authService.LoginAsync(model.Email, model.Password);
            if (user == null)
            {
                return Unauthorized(new { message = "Invalid credentials" });
            }

            var token = await _authService.GenerateJwtToken(user);


            // Sæt cookie-udløbstiden til 2 timer fra nu
            var expirationTime = DateTime.UtcNow.AddHours(2); // Udløber om 2 timer
            Response.Cookies.Append("jwtToken", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax,
                Expires = expirationTime
            });

            return Ok(new { message = "Login successful", token });
        }



    }
}
