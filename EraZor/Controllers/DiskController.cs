using EraZor.Data;
using EraZor.DTOs;
using EraZor.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EraZor.Controllers
{
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
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DiskReadDto>>> GetDisks()
        {
            var disks = await _context.Disks
                .Select(d => new DiskReadDto
                {
                    DiskID = d.DiskID,
                    Type = d.Type,
                    Capacity = d.Capacity,
                    Path = d.Path,
                    SerialNumber = d.SerialNumber,
                    Manufacturer = d.Manufacturer
                })
                .ToListAsync();

            return Ok(disks);
        }

        // GET: api/Disks/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<DiskReadDto>> GetDisk(int id)
        {
            var disk = await _context.Disks.FindAsync(id);

            if (disk == null)
            {
                return NotFound();
            }

            var diskDto = new DiskReadDto
            {
                DiskID = disk.DiskID,
                Type = disk.Type,
                Capacity = disk.Capacity,
                Path = disk.Path,
                SerialNumber = disk.SerialNumber,
                Manufacturer = disk.Manufacturer
            };

            return Ok(diskDto);
        }

        // POST: api/Disks
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateDisk([FromBody] DiskCreateDto dto)
        {
            try
            {
                // Opret ny Disk baseret på DTO
                var disk = new Disk
                {
                    Type = dto.Type,
                    Capacity = dto.Capacity,
                    Path = dto.Path,
                    SerialNumber = dto.SerialNumber,
                    Manufacturer = dto.Manufacturer
                };

                _context.Disks.Add(disk);
                await _context.SaveChangesAsync();

                var result = new DiskReadDto
                {
                    DiskID = disk.DiskID,
                    Type = disk.Type,
                    Capacity = disk.Capacity,
                    Path = disk.Path,
                    SerialNumber = disk.SerialNumber,
                    Manufacturer = disk.Manufacturer
                };

                return CreatedAtAction(nameof(GetDisk), new { id = result.DiskID }, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while saving the disk.", details = ex.Message });
            }
        }


        // PUT: api/Disks/5
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDisk(int id, [FromBody] DiskCreateDto dto)
        {
            var existingDisk = await _context.Disks.FindAsync(id);

            if (existingDisk == null)
            {
                return NotFound();
            }

            // Opdater eksisterende disk med DTO-data
            existingDisk.Type = dto.Type;
            existingDisk.Capacity = dto.Capacity;
            existingDisk.Path = dto.Path;
            existingDisk.SerialNumber = dto.SerialNumber;
            existingDisk.Manufacturer = dto.Manufacturer;

            _context.Entry(existingDisk).State = EntityState.Modified;

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
        [Authorize]
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
}
