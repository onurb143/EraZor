using EraZor.DTOs;
using EraZor.Interfaces;
using EraZor.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EraZor.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DisksController : ControllerBase
    {
        private readonly IDiskService _diskService;

        public DisksController(IDiskService diskService)
        {
            _diskService = diskService;
        }

        // GET: api/Disks
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DiskReadDto>>> GetDisks()
        {
            var disks = await _diskService.GetDisksAsync();

            // Map Disk til DiskReadDto (manuelt her for eksempel)
            var result = disks.Select(d => new DiskReadDto
            {
                DiskID = d.DiskID,
                Type = d.Type,
                Capacity = d.Capacity,
                Path = d.Path,
                SerialNumber = d.SerialNumber,
                Manufacturer = d.Manufacturer
            }).ToList();

            return Ok(result);
        }


        // GET: api/Disks/{id}
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<DiskReadDto>> GetDisk(int id)
        {
            var disk = await _diskService.GetDiskByIdAsync(id);

            if (disk == null)
            {
                return NotFound($"Disk med ID {id} blev ikke fundet.");
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
            if (dto == null || string.IsNullOrEmpty(dto.Type))
            {
                return BadRequest("Disk type er påkrævet.");
            }

            var disk = new Disk
            {
                Type = dto.Type,
                Capacity = dto.Capacity,
                Path = dto.Path,
                SerialNumber = dto.SerialNumber,
                Manufacturer = dto.Manufacturer
            };

            await _diskService.AddDiskAsync(disk);

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

        // DELETE: api/Disks/{id}
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDisk(int id)
        {
            var exists = await _diskService.DiskExistsAsync(id);
            if (!exists)
            {
                return NotFound($"Disk med ID {id} kunne ikke findes.");
            }

            await _diskService.DeleteDiskAsync(id);

            return NoContent();
        }
    }
}
