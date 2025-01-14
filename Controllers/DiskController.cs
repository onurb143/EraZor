using EraZor.DTOs;
using EraZor.Interfaces;
using EraZor.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EraZor.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DisksController : ControllerBase
    {
        private readonly IDiskService _diskRepository;

        public DisksController(IDiskService diskRepository)
        {
            _diskRepository = diskRepository;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DiskReadDto>>> GetDisks()
        {
            var disks = await _diskRepository.GetDisksAsync();
            var diskDtos = disks.Select(d => new DiskReadDto
            {
                DiskID = d.DiskID,
                Type = d.Type,
                Capacity = d.Capacity,
                Path = d.Path,
                SerialNumber = d.SerialNumber,
                Manufacturer = d.Manufacturer
            });

            return Ok(diskDtos);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<DiskReadDto>> GetDisk(int id)
        {
            var disk = await _diskRepository.GetDiskByIdAsync(id);
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

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateDisk([FromBody] DiskCreateDto dto)
        {
            var disk = new Disk
            {
                Type = dto.Type,
                Capacity = dto.Capacity,
                Path = dto.Path,
                SerialNumber = dto.SerialNumber,
                Manufacturer = dto.Manufacturer
            };

            await _diskRepository.AddDiskAsync(disk);

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

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDisk(int id)
        {
            var exists = _diskRepository.DiskExists(id);
            if (!exists)
            {
                return NotFound();
            }

            await _diskRepository.DeleteDiskAsync(id);

            return NoContent();
        }
    }
}
