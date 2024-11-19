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

    [HttpGet("test-db-connection")]
    public async Task<IActionResult> TestDbConnection()
    {
        try
        {
            var testResult = await _context.Disks.FirstOrDefaultAsync();
            return Ok("Database connection successful.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Database connection failed: {ex.Message}");
        }
    }

    // GET: api/Disks
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Disk>>> GetDisks()
    {
        return await _context.Disks.ToListAsync();
    }

    // GET: api/Disks/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Disk>> GetDisk(int id)
    {
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
        _context.Disks.Add(disk);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetDisk), new { id = disk.diskID  }, disk);
    }

    // PUT: api/Disks/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutDisk(int id, Disk disk)
    {
        if (id != disk.diskID)
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
        return _context.Disks.Any(e => e.diskID == id);
    }
}
