// Importerer nødvendige namespaces for at arbejde med MVC, JWT, og Identity
using Microsoft.AspNetCore.Mvc;  // Bruges til at oprette controller-klasser og actions i ASP.NET Core MVC
using Microsoft.IdentityModel.Tokens;  // Bruges til at arbejde med JWT (JSON Web Token) og signering af tokens
using System.IdentityModel.Tokens.Jwt;  // Bruges til at oprette, validere og analysere JWT-tokens
using System.Security.Claims;  // Bruges til at oprette og arbejde med "claims" i JWT-tokens
using System.Text;  // Bruges til at arbejde med tekst, især når man konverterer tekst til bytes, som kræves ved signering af JWT
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;  // Bruges til at arbejde med Identity-systemet for brugerhåndtering i ASP.NET Core (Login, Register osv.)


namespace WebKlient.Controllers
{
    // Definerer controller-ruten som 'api/auth'
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        // Afhængigheder til brugerhåndtering og sign-in
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _configuration;

        // Konstruktør til at injicere afhængighederne
        public AuthController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        // Login endpoint der modtager brugerens email og password
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            // Tjekker om model er null eller har tomme felter
            if (model == null || string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
            {
                return BadRequest(new { message = "Email and password are required." });
            }

            try
            {
                // Henter brugeren fra databasen baseret på email
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    return Unauthorized(new { message = "Invalid credentials" });
                }

                // Tjekker om loginoplysningerne er korrekte
                var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
                if (!result.Succeeded)
                {
                    return Unauthorized(new { message = "Invalid credentials" });
                }

                // Generer JWT-token for brugeren
                var token = GenerateJwtToken(user);

                // Gemmer JWT-tokenet som en cookie
                Response.Cookies.Append("jwtToken", token, new CookieOptions
                {
                    HttpOnly = true, // Forhindrer adgang via JavaScript
                    Secure = true, // Kræver HTTPS
                    SameSite = SameSiteMode.Strict, // CSRF-beskyttelse
                    Expires = DateTime.UtcNow.AddMinutes(60) // Udløbsperiode
                });

                // Returnerer en succesmeddelelse med token
                return Ok(new { message = "Login successful", token });
            }
            catch (Exception ex)
            {
                // Logfejl ved login
                Console.Error.WriteLine($"Error during login: {ex.Message}");
                return StatusCode(500, new { message = "An error occurred during login." });
            }
        }

        // Metode til at generere JWT-token
        private string GenerateJwtToken(IdentityUser user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");

            // Henter hemmelighed til signering af token fra konfigurationen
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Secret"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Opretter claims til tokenet
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // Tilføjer brugerens roller som claims
            var roles = _userManager.GetRolesAsync(user).Result;
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            // Tilføjer audience (målgruppe) claims hvis det er defineret i konfigurationen
            var audienceSection = jwtSettings.GetSection("Audience").Get<string[]>();
            if (audienceSection != null)
            {
                claims.AddRange(audienceSection.Select(aud => new Claim(JwtRegisteredClaimNames.Aud, aud)));
            }

            // Angiver hvor lang tid tokenet skal være gyldigt
            var expiresInMinutes = int.TryParse(jwtSettings["ExpiresInMinutes"], out var expires) ? expires : 60;

            // Opretter JWT-token
            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: null, // Audiences bliver claims
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiresInMinutes),
                signingCredentials: creds
            );

            // Returnerer det genererede token som en string
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    // LoginModel klasse der repræsenterer brugerens loginoplysninger
    public class LoginModel
    {
        // Brugerens email (required i stedet for nullable)
        [Required(ErrorMessage = "Email is required.")]
        public required string Email { get; set; }

        // Brugerens password (required i stedet for nullable)
        [Required(ErrorMessage = "Password is required.")]
        public required string Password { get; set; }
    }
}
