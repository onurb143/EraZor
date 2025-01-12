using EraZor.Data;
using EraZor.DTOs;
using EraZor.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EraZor.Controllers
{
    // Definerer denne controller som en API-controller og sætter base-ruten for API-endepunkterne
    [ApiController]
    [Route("api/[controller]")]
    public class DisksController : ControllerBase
    {
        // Dependency Injection af DataContext for at interagere med databasen
        private readonly DataContext _context;

        public DisksController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Disks - Henter alle diske fra databasen
        [Authorize] // Sikrer at kun autentificerede brugere kan tilgå denne endpoint
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DiskReadDto>>> GetDisks()
        {
            // Henter alle diske og mapper dem til DTO'er for at sikre renere og sikrere output
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

            // Returnerer listen af diske som en HTTP 200 OK respons
            return Ok(disks);
        }

        // GET: api/Disks/5 - Henter en specifik disk baseret på ID
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<DiskReadDto>> GetDisk(int id)
        {
            // Finder disk baseret på ID, returnerer NotFound hvis ingen disk findes
            var disk = await _context.Disks.FindAsync(id);

            if (disk == null)
            {
                return NotFound();
            }

            // Mapper den fundne disk til en DTO
            var diskDto = new DiskReadDto
            {
                DiskID = disk.DiskID,
                Type = disk.Type,
                Capacity = disk.Capacity,
                Path = disk.Path,
                SerialNumber = disk.SerialNumber,
                Manufacturer = disk.Manufacturer
            };

            // Returnerer den fundne disk som en HTTP 200 OK respons
            return Ok(diskDto);
        }

        // POST: api/Disks - Opretter en ny disk
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateDisk([FromBody] DiskCreateDto dto)
        {
            try
            {
                // Opretter en ny Disk baseret på de indkommende DTO-data
                var disk = new Disk
                {
                    Type = dto.Type,
                    Capacity = dto.Capacity,
                    Path = dto.Path,
                    SerialNumber = dto.SerialNumber,
                    Manufacturer = dto.Manufacturer
                };

                // Tilføjer den nye disk til databasen og gemmer ændringerne
                _context.Disks.Add(disk);
                await _context.SaveChangesAsync();

                // Mapper den nye oprettede disk til en DTO for at returnere til klienten
                var result = new DiskReadDto
                {
                    DiskID = disk.DiskID,
                    Type = disk.Type,
                    Capacity = disk.Capacity,
                    Path = disk.Path,
                    SerialNumber = disk.SerialNumber,
                    Manufacturer = disk.Manufacturer
                };

                // Returnerer en HTTP 201 Created status med den oprettede disk og dens placering
                return CreatedAtAction(nameof(GetDisk), new { id = result.DiskID }, result);
            }
            catch (Exception ex)
            {
                // Returnerer en HTTP 500 fejl, hvis der opstår en fejl under oprettelsen
                return StatusCode(500, new { message = "An error occurred while saving the disk.", details = ex.Message });
            }
        }

        // DELETE: api/Disks/5 - Sletter en specifik disk baseret på ID
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDisk(int id)
        {
            // Finder disk baseret på ID, returnerer NotFound hvis ingen disk findes
            var disk = await _context.Disks.FindAsync(id);
            if (disk == null)
            {
                return NotFound();
            }

            // Fjerner den fundne disk og gemmer ændringerne i databasen
            _context.Disks.Remove(disk);
            await _context.SaveChangesAsync();

            // Returnerer HTTP 204 No Content status for at indikere en succesfuld sletning
            return NoContent();
        }

        // Hjælpermetode for at tjekke om en disk eksisterer i databasen
        private bool DiskExists(int id)
        {
            return _context.Disks.Any(e => e.DiskID == id);
        }
    }
}
