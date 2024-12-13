using EraZor.Interfaces;
using EraZor.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class WipeMethodsController : ControllerBase
{
    private readonly IWipeMethodService _wipeMethodService;

    // Dependency Injection for IWipeMethodService
    public WipeMethodsController(IWipeMethodService wipeMethodService)
    {
        _wipeMethodService = wipeMethodService;
    }

    // GET: api/WipeMethods - Henter alle slettemetoder
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<WipeMethod>>> GetWipeMethods()
    {
        var wipeMethods = await _wipeMethodService.GetAllWipeMethodsAsync();
        return Ok(wipeMethods);
    }

    // DELETE: api/WipeMethods/5 - Sletter en slettemetode baseret på ID
    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteWipeMethod(int id)
    {
        var wipeMethod = await _wipeMethodService.GetWipeMethodByIdAsync(id);
        if (wipeMethod == null)
        {
            return NotFound();
        }

        await _wipeMethodService.DeleteWipeMethodAsync(id);
        return NoContent();
    }
}
