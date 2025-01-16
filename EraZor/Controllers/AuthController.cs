using EraZor.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Erazor.DTOs;
using EraZor.Model;

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
                Console.WriteLine("Model is null or email/password is empty.");
                return BadRequest(new AuthResponse { Message = "Email and password are required." });
            }

            var user = await _authService.LoginAsync(model.Email, model.Password);
            if (user == null)
            {
                Console.WriteLine("User is null.");
                return Unauthorized(new AuthResponse { Message = "Invalid credentials" });
            }

            var token = await _authService.GenerateJwtToken(user);
            Console.WriteLine($"Generated token: {token}");

            var expirationTime = DateTime.UtcNow.AddHours(2);
            Response.Cookies.Append("jwtToken", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax,
                Expires = expirationTime
            });

            return Ok(new AuthResponse { Message = "Login successful", Token = token });
        }






    }
}
