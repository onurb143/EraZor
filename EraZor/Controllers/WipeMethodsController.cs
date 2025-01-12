using EraZor.Data;
using EraZor.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController] // Gør denne klasse til en API controller
[Route("api/[controller]")] // Sætter ruten til at være baseret på controllerens navn, f.eks. api/WipeMethods
public class WipeMethodsController : ControllerBase
{
    private readonly DataContext _context;

    // Konstruktor til controlleren, som modtager DataContext for databaseinteraktion
    public WipeMethodsController(DataContext context)
    {
        _context = context;
    }

    // GET: api/WipeMethods - Henter alle slette metoder
    [Authorize] // Beskytter ruten ved kun at tillade autoriserede brugere at få adgang
    [HttpGet] // HTTP GET metode
    public async Task<ActionResult<IEnumerable<WipeMethod>>> GetWipeMethods()
    {
        // Henter alle slette metoder fra databasen og returnerer dem som en liste
        return await _context.WipeMethods.ToListAsync();
    }

    // DELETE: api/WipeMethods/5 - Sletter en slette metode baseret på ID
    [Authorize] // Beskytter sletningen ved kun at tillade autoriserede brugere at slette
    [HttpDelete("{id}")] // HTTP DELETE metode
    public async Task<IActionResult> DeleteWipeMethod(int id)
    {
        // Finder den slette metode med det angivne ID
        var wipeMethod = await _context.WipeMethods.FindAsync(id);
        if (wipeMethod == null)
        {
            return NotFound(); // Returnerer 404 Not Found hvis slette metoden ikke findes
        }

        // Fjerner slette metoden fra databasen
        _context.WipeMethods.Remove(wipeMethod);
        await _context.SaveChangesAsync(); // Gemmer ændringerne i databasen

        return NoContent();  // Returnerer en tom svarkode (204 No Content) for at indikere succesfuld sletning
    }
}
