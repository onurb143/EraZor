using EraZor.Data;
using EraZor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class WipeMethodsController : ControllerBase
{
    private readonly DataContext _context;

    public WipeMethodsController(DataContext context)
    {
        _context = context;
    }

    // GET: api/WipeMethods
    [HttpGet]
    public async Task<ActionResult<IEnumerable<WipeMethod>>> GetWipeMethods()
    {
        return await _context.WipeMethods.ToListAsync();
    }

    // GET: api/WipeMethods/5
    [HttpGet("{id}")]
    public async Task<ActionResult<WipeMethod>> GetWipeMethod(int id)
    {
        var wipeMethod = await _context.WipeMethods.FindAsync(id);

        if (wipeMethod == null)
        {
            return NotFound();
        }

        return wipeMethod;
    }

    // POST: api/WipeMethods
    [HttpPost]
    public async Task<ActionResult<WipeMethod>> PostWipeMethod(WipeMethod wipeMethod)
    {
        _context.WipeMethods.Add(wipeMethod);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetWipeMethod), new { id = wipeMethod.WipeMethodID }, wipeMethod);
    }

    // PUT: api/WipeMethods/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutWipeMethod(int id, WipeMethod wipeMethod)
    {
        if (id != wipeMethod.WipeMethodID)
        {
            return BadRequest();
        }

        _context.Entry(wipeMethod).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!WipeMethodExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // DELETE: api/WipeMethods/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteWipeMethod(int id)
    {
        var wipeMethod = await _context.WipeMethods.FindAsync(id);
        if (wipeMethod == null)
        {
            return NotFound();
        }

        _context.WipeMethods.Remove(wipeMethod);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool WipeMethodExists(int id)
    {
        return _context.WipeMethods.Any(e => e.WipeMethodID == id);
    }
}
