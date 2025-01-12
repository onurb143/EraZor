using EraZor.Data;
using EraZor.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EraZor.DTO;
using Microsoft.AspNetCore.Authorization;


[ApiController]
[Route("api/[controller]")]
public class WipeReportsController : ControllerBase
{
    private readonly DataContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    // Constructor med injektion af UserManager
    public WipeReportsController(DataContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetWipeReports()
    {
        var wipeReports = await _context.WipeJobs
            .Include(wj => wj.WipeMethod)
            .Include(wj => wj.Disk)
            .Include(wj => wj.PerformedByUser)
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
                OverwritePasses = wj.WipeMethod.OverwritePass,
                PerformedBy = wj.PerformedByUser.UserName  // Referring to IdentityUser
            })
            .ToListAsync();

        return Ok(wipeReports);
    }


    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateWipeReport([FromBody] WipeReportCreateDto dto)
    {
        if (dto == null)
        {
            return BadRequest("Invalid data.");
        }

        // Hent den nuværende bruger
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return Unauthorized();
        }

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
            StartTime = DateTime.SpecifyKind(dto.StartTime, DateTimeKind.Utc),
            EndTime = DateTime.SpecifyKind(dto.EndTime, DateTimeKind.Utc),
            Status = dto.Status,
            DiskId = disk.DiskID,
            WipeMethodId = wipeMethod.WipeMethodID,
            PerformedByUserId = user.Id // Gem brugerens ID
        };

        // Gem WipeJob i databasen
        await _context.WipeJobs.AddAsync(wipeJob);
        await _context.SaveChangesAsync();

        // DTO for respons
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
            OverwritePasses = wipeMethod.OverwritePass,
            PerformedBy = user.UserName // Returner brugerens navn
        };

        return CreatedAtAction(nameof(GetWipeReports), new { id = result.WipeJobId }, result);
    }


    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteWipeReport(int id)
    {
        var wipeJob = await _context.WipeJobs
            .FirstOrDefaultAsync(wj => wj.WipeJobId == id);

        if (wipeJob == null)
        {
            return NotFound(); // Returner 404, hvis rapporten ikke findes
        }

        _context.WipeJobs.Remove(wipeJob);
        await _context.SaveChangesAsync();

        return NoContent(); // Returner 204 No Content på succes
    }



}
