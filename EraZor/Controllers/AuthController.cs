using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace WebKlient.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _configuration;

        public AuthController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return Unauthorized(new { message = "Invalid credentials" });

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if (!result.Succeeded)
                return Unauthorized(new { message = "Invalid credentials" });

            // Generate JWT
            var token = GenerateJwtToken(user);
            return Ok(new { token });
        }

        private string GenerateJwtToken(IdentityUser user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");

            // Debugging logs
            Console.WriteLine($"Issuer: {jwtSettings["Issuer"]}");
            Console.WriteLine($"Secret: {jwtSettings["Secret"]}");

            // Validate Secret
            var secret = jwtSettings["Secret"];
            if (string.IsNullOrEmpty(secret))
            {
                throw new InvalidOperationException("JWT Secret is missing in configuration.");
            }

            // Validate Audience
            var audienceSection = jwtSettings.GetSection("Audience").Get<string[]>();
            if (audienceSection == null || audienceSection.Length == 0)
            {
                throw new InvalidOperationException("JWT Audience is missing in configuration.");
            }

            // Prepare key and credentials
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Define claims
            var claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Sub, user.Id),
        new Claim(JwtRegisteredClaimNames.Email, user.Email),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

            // Create the token
            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: audienceSection[0], // Ensure there is at least one audience
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(int.Parse(jwtSettings["ExpiresInMinutes"] ?? "60")),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


    }

    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
