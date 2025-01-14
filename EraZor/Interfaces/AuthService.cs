using EraZor.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EraZor.Repositories
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

      public async Task<IdentityUser> LoginAsync(string email, string password)
{
    var user = await _userManager.FindByEmailAsync(email);
    if (user == null)
        return null;

    var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
    return result.Succeeded ? user : null;
}

public async Task<bool> RegisterAsync(string email, string password)
{
    var user = new IdentityUser { Email = email, UserName = email };
    var result = await _userManager.CreateAsync(user, password);
    return result.Succeeded;
}

        public async Task<string> GenerateJwtToken(IdentityUser user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Secret"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Sub, user.Id),
        new Claim(JwtRegisteredClaimNames.Email, user.Email),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

            var roles = await _userManager.GetRolesAsync(user);
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var audienceSection = jwtSettings.GetSection("Audience").Get<string[]>();
            if (audienceSection != null)
            {
                claims.AddRange(audienceSection.Select(aud => new Claim(JwtRegisteredClaimNames.Aud, aud)));
            }

            // Sæt udløbstiden til 2 timer i stedet for den tidligere konfiguration
            var expirationTime = DateTime.UtcNow.AddHours(2);

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"], // Tilføjet audience
                claims: claims,
                expires: expirationTime, // Ændret til 2 timer frem for UTC nu
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


    }
}
