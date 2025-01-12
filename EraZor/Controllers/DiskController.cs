using EraZor.Data;
using EraZor.DTOs;
using EraZor.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EraZor.Controllers
{
    // Defines this controller as an API controller and sets the base route for the API endpoints
    [ApiController]
    [Route("api/[controller]")]
    public class DisksController : ControllerBase
    {
        // Dependency injection for DataContext to interact with the database
        private readonly DataContext _context;

        public DisksController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Disks - Fetches all disks from the database
        [Authorize] // Ensures only authenticated users can access this endpoint
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DiskReadDto>>> GetDisks()
        {
            // Fetching disks and mapping to DTOs for safer and cleaner output
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

            // Returns the list of disks as an HTTP 200 OK response
            return Ok(disks);
        }

        // GET: api/Disks/5 - Fetches a specific disk by ID
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<DiskReadDto>> GetDisk(int id)
        {
            // Find disk by ID, returns NotFound if no disk is found
            var disk = await _context.Disks.FindAsync(id);

            if (disk == null)
            {
                return NotFound();
            }

            // Mapping the found disk to DTO
            var diskDto = new DiskReadDto
            {
                DiskID = disk.DiskID,
                Type = disk.Type,
                Capacity = disk.Capacity,
                Path = disk.Path,
                SerialNumber = disk.SerialNumber,
                Manufacturer = disk.Manufacturer
            };

            // Returns the disk as an HTTP 200 OK response
            return Ok(diskDto);
        }

        // POST: api/Disks - Creates a new disk entry
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateDisk([FromBody] DiskCreateDto dto)
        {
            try
            {
                // Create new Disk object based on the incoming DTO data
                var disk = new Disk
                {
                    Type = dto.Type,
                    Capacity = dto.Capacity,
                    Path = dto.Path,
                    SerialNumber = dto.SerialNumber,
                    Manufacturer = dto.Manufacturer
                };

                // Add the new disk to the database context and save changes
                _context.Disks.Add(disk);
                await _context.SaveChangesAsync();

                // Map the newly created disk to a DTO to return to the client
                var result = new DiskReadDto
                {
                    DiskID = disk.DiskID,
                    Type = disk.Type,
                    Capacity = disk.Capacity,
                    Path = disk.Path,
                    SerialNumber = disk.SerialNumber,
                    Manufacturer = disk.Manufacturer
                };

                // Returns an HTTP 201 Created status with the result and location of the new resource
                return CreatedAtAction(nameof(GetDisk), new { id = result.DiskID }, result);
            }
            catch (Exception ex)
            {
                // Returns an HTTP 500 error if something goes wrong
                return StatusCode(500, new { message = "An error occurred while saving the disk.", details = ex.Message });
            }
        }

        // DELETE: api/Disks/5 - Deletes a specific disk by ID
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDisk(int id)
        {
            // Find disk by ID to delete, returns NotFound if no disk is found
            var disk = await _context.Disks.FindAsync(id);
            if (disk == null)
            {
                return NotFound();
            }

            // Remove the found disk and save changes to the database
            _context.Disks.Remove(disk);
            await _context.SaveChangesAsync();

            // Returns HTTP 204 No Content status indicating successful deletion
            return NoContent();
        }

        // Helper method to check if a disk exists in the database
        private bool DiskExists(int id)
        {
            return _context.Disks.Any(e => e.DiskID == id);
        }
    }
}
