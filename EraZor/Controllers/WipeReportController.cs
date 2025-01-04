using EraZor.Data;
using EraZor.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using WebKlient.DTO_s;

namespace EraZor.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WipeReportsController : ControllerBase
    {
        private readonly DataContext _context;

        public WipeReportsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/WipeReports
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetWipeReports()
        {
            var wipeReports = await _context.WipeJobs
                .Include(wj => wj.WipeMethod)
                .Include(wj => wj.Disk)
                .Select(wj => new WipeReportReadDto
                {
                    WipeJobId = wj.WipeJobId,
                    StartTime = wj.StartTime,
                    EndTime = wj.EndTime,
                    Status = wj.Status,
                    DiskType = wj.Disk.Type,
                    Capacity = wj.Disk.Capacity,
                    SerialNumber = wj.Disk.SerialNumber,
                    Manufacturer = wj.Disk.Manufacturer,
                    WipeMethodName = wj.WipeMethod.Name,
                    OverwritePasses = wj.WipeMethod.OverwritePass
                })
                .ToListAsync();

            return Ok(wipeReports);
        }


        // POST: api/WipeReports
        [HttpPost]
        public async Task<IActionResult> CreateWipeReport([FromBody] WipeReportCreateDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Invalid data.");
            }

            // Konverter StartTime og EndTime til UTC
            var startTime = DateTime.SpecifyKind(dto.StartTime, DateTimeKind.Utc);
            var endTime = DateTime.SpecifyKind(dto.EndTime, DateTimeKind.Utc);

            // Find disk baseret på serienummer
            var disk = await _context.Disks.FirstOrDefaultAsync(d => d.SerialNumber == dto.SerialNumber);
            if (disk == null)
            {
                return BadRequest("Disk not found.");
            }

            // Find slettemetode baseret på navn
            var wipeMethod = await _context.WipeMethods.FirstOrDefaultAsync(wm => wm.Name == dto.WipeMethodName);
            if (wipeMethod == null)
            {
                return BadRequest("Wipe method not found.");
            }

            // Opret WipeJob
            var wipeJob = new WipeJob
            {
                StartTime = startTime,
                EndTime = endTime,
                Status = dto.Status,
                DiskId = disk.DiskID,
                WipeMethodId = wipeMethod.WipeMethodID
            };

            // Gem i databasen
            await _context.WipeJobs.AddAsync(wipeJob);
            await _context.SaveChangesAsync();

            // Returnér data som en DTO
            var result = new WipeReportReadDto
            {
                WipeJobId = wipeJob.WipeJobId,
                StartTime = wipeJob.StartTime,
                EndTime = wipeJob.EndTime,
                Status = wipeJob.Status,
                DiskType = disk.Type,
                Capacity = disk.Capacity,
                SerialNumber = disk.SerialNumber,
                Manufacturer = disk.Manufacturer,
                WipeMethodName = wipeMethod.Name,
                OverwritePasses = wipeMethod.OverwritePass
            };

            return CreatedAtAction(nameof(GetWipeReports), new { id = result.WipeJobId }, result);
        }






    }
}
