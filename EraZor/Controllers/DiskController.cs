using EraZor.Data;
using EraZor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class DisksController : ControllerBase
{
    private readonly DataContext _context;

    public DisksController(DataContext context)
    {
        _context = context;
    }

    // GET: api/Disks
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Disk>>> GetDisks()
    {
        // Hent alle diske fra databasen
        return await _context.Disks.ToListAsync();
    }

    // GET: api/Disks/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Disk>> GetDisk(int id)
    {
        // Hent en specifik disk baseret på ID
        var disk = await _context.Disks.FindAsync(id);

        if (disk == null)
        {
            return NotFound();
        }

        return disk;
    }

    // POST: api/Disks
    [HttpPost]
    public async Task<ActionResult<Disk>> PostDisk(Disk disk)
    {
        // Tilføj en ny disk til databasen
        _context.Disks.Add(disk);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetDisk), new { id = disk.DiskID }, disk);
    }

    // PUT: api/Disks/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutDisk(int id, Disk disk)
    {
        // Tjek om ID i URL'en matcher diskens ID
        if (id != disk.DiskID)
        {
            return BadRequest();
        }

        _context.Entry(disk).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!DiskExists(id))
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

    // DELETE: api/Disks/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDisk(int id)
    {
        var disk = await _context.Disks.FindAsync(id);
        if (disk == null)
        {
            return NotFound();
        }

        _context.Disks.Remove(disk);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool DiskExists(int id)
    {
        return _context.Disks.Any(e => e.DiskID == id);
    }
}
