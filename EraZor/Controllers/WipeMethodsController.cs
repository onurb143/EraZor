using Erazor.DTOs;
using EraZor.Interfaces;
using EraZor.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Erazor.Controllers
{
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
            try
            {
                var wipeMethods = await _wipeMethodService.GetAllWipeMethodsAsync();
                return Ok(wipeMethods);
            }
            catch (Exception ex)
            {
                // Håndterer eventuelle fejl i service kaldet
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE: api/WipeMethods/5 - Sletter en slettemetode baseret på ID
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWipeMethod(int id)
        {
            try
            {
                var wipeMethod = await _wipeMethodService.GetWipeMethodByIdAsync(id);
                if (wipeMethod == null)
                {
                    return NotFound(new { Message = "Wipe method not found" });
                }

                await _wipeMethodService.DeleteWipeMethodAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}

