using EraZor.Data;
using EraZor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

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
        [HttpGet]
        public async Task<IActionResult> GetWipeReports()
        {
            var wipeReports = await _context.WipeJobs
                .Include(wj => wj.WipeMethod)
                .Include(wj => wj.Disk)
                .Select(wj => new WipeReport
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
                    OverwritePasses = wj.WipeMethod.OverwritePass,
                    PerformedBy = wj.UserId // Reference til brugeren
                })
                .ToListAsync();

            return Ok(wipeReports);
        }

        // POST: api/WipeReports
        [HttpPost]
        public async Task<IActionResult> CreateWipeReport([FromBody] WipeReport wipeReport)
        {
            if (wipeReport == null)
            {
                return BadRequest("Invalid wipe report data.");
            }

            // Find disk baseret på serienummer
            var disk = await _context.Disks.FirstOrDefaultAsync(d => d.SerialNumber == wipeReport.SerialNumber);
            if (disk == null)
            {
                return BadRequest("Disk not found.");
            }

            // Find slettemetode baseret på navn
            var wipeMethod = await _context.WipeMethods.FirstOrDefaultAsync(wm => wm.Name == wipeReport.WipeMethodName);
            if (wipeMethod == null)
            {
                return BadRequest("Wipe method not found.");
            }

            // Find bruger baseret på PerformedBy (UserName fra Identity Framework)
            var user = await _context.Users.FirstOrDefaultAsync(u => u.NormalizedUserName == wipeReport.PerformedBy.ToUpper());
            if (user == null)
            {
                return BadRequest("User not found.");
            }

            // Opret en ny WipeJob
            var wipeJob = new WipeJob
            {
                StartTime = wipeReport.StartTime,
                EndTime = wipeReport.EndTime,
                Status = wipeReport.Status,
                DiskId = disk.DiskID,
                WipeMethodId = wipeMethod.WipeMethodID,
                UserId = user.Id // Brug det korrekte Id fra AspNetUsers-tabellen
            };

            // Tilføj WipeJob til databasen
            await _context.WipeJobs.AddAsync(wipeJob);
            await _context.SaveChangesAsync();

            // Returnér den oprettede rapport
            return CreatedAtAction(nameof(GetWipeReports), new { id = wipeJob.WipeJobId }, wipeJob);
        }
    }
}
