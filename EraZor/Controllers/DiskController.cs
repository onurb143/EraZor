using EraZor.Data;
using EraZor.DTOs;
using EraZor.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace EraZor.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class DisksController : ControllerBase
    {
        private readonly DataContext _context;

        public DisksController(DataContext context)
        {
            _context = context;
        }

        // GET: api/v1/Disks
        [Authorize]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Client)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetDisks()
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

            var response = disks.Select(d => new
            {
                data = d,
                links = new[]
                {
                    new { rel = "self", href = Url.Action(nameof(GetDisk), new { id = d.DiskID }) },
                    new { rel = "delete", href = Url.Action(nameof(DeleteDisk), new { id = d.DiskID }) },
                    new { rel = "update", href = Url.Action(nameof(UpdateDisk), new { id = d.DiskID }) }
                }
            });

            return Ok(response);
        }

        // GET: api/v1/Disks/{id}
        [Authorize]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Client)]
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetDisk(int id)
        {
            var disk = await _context.Disks.FindAsync(id);

            if (disk == null)
            {
                return NotFound(new { error = "Not Found", message = "The specified disk was not found." });
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

            var response = new
            {
                data = diskDto,
                links = new[]
                {
                    new { rel = "self", href = Url.Action(nameof(GetDisk), new { id = diskDto.DiskID }) },
                    new { rel = "delete", href = Url.Action(nameof(DeleteDisk), new { id = diskDto.DiskID }) },
                    new { rel = "update", href = Url.Action(nameof(UpdateDisk), new { id = diskDto.DiskID }) }
                }
            };

            return Ok(response);
        }

        // POST: api/v1/Disks
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateDisk([FromBody] DiskCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
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
                return StatusCode(500, new
                {
                    error = "Internal Server Error",
                    message = "An error occurred while saving the disk.",
                    details = ex.Message
                });
            }
        }

        // PUT: api/v1/Disks/{id}
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDisk(int id, [FromBody] DiskCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingDisk = await _context.Disks.FindAsync(id);

            if (existingDisk == null)
            {
                return NotFound(new { error = "Not Found", message = "The specified disk was not found." });
            }

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
                    return NotFound(new { error = "Not Found", message = "The specified disk was not found." });
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/v1/Disks/{id}
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDisk(int id)
        {
            var disk = await _context.Disks.FindAsync(id);
            if (disk == null)
            {
                return NotFound(new { error = "Not Found", message = "The specified disk was not found." });
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

    public class DiskCreateDto
    {
        [Required]
        public string Type { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Capacity must be greater than 0.")]
        public int Capacity { get; set; }

        [Required]
        [MaxLength(200)]
        public string Path { get; set; }

        [Required]
        [MaxLength(100)]
        public string SerialNumber { get; set; }

        [Required]
        [MaxLength(100)]
        public string Manufacturer { get; set; }
    }
}
