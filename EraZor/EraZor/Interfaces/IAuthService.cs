using Microsoft.AspNetCore.Identity;

namespace EraZor.Interfaces
{
    public interface IAuthService
    {
        Task<IdentityUser> LoginAsync(string email, string password); // Returnerer en bruger ved login
        Task<bool> RegisterAsync(string email, string password); // Opretter en ny bruger
        Task<string> GenerateJwtToken(IdentityUser user); // Genererer JWT-token
    }
}
