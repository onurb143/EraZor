using EraZor.Data;
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

    // Constructor injektion af DataContext og UserManager
    public WipeReportsController(DataContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;  // DataContext til at interagere med databasen
        _userManager = userManager;  // UserManager til at håndtere brugerautentifikation
    }

    // GET: api/WipeReports
    [Authorize]  // Beskytter metoden med Authorization, kræver login
    [HttpGet]
    public async Task<IActionResult> GetWipeReports()
    {
        var wipeReports = await _context.WipeJobs
            .Include(wj => wj.WipeMethod)  // Inkluderer slettemetodeinformation
            .Include(wj => wj.Disk)  // Inkluderer diskinformation
            .Include(wj => wj.PerformedByUser)  // Inkluderer brugerinfo
            .Select(wj => new WipeReportReadDto  // Mapper WipeJobs til DTO for effektiv dataoverførsel
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
                PerformedBy = wj.PerformedByUser.UserName  // Brugerens navn som præstationsansvarlig
            })
            .ToListAsync();

        return Ok(wipeReports);  // Returnerer listen af rapporter i et OK-svar
    }

    // POST: api/WipeReports
    [Authorize]  // Kræver autorisation for at oprette en ny wipe report
    [HttpPost]
    public async Task<IActionResult> CreateWipeReport([FromBody] WipeReportCreateDto dto)
    {
        if (dto == null)  // Tjekker om input er null
        {
            return BadRequest("Invalid data.");  // Returnerer en fejl, hvis dataen er ugyldig
        }

        // Henter den nuværende bruger
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return Unauthorized();  // Returnerer Unauthorized, hvis brugeren ikke er fundet
        }

        // Finder disk baseret på serienummer
        var disk = await _context.Disks.FirstOrDefaultAsync(d => d.SerialNumber == dto.SerialNumber);
        if (disk == null)
        {
            return BadRequest("Disk not found.");  // Returnerer fejl, hvis disk ikke findes
        }

        // Finder slettemetoden baseret på navn
        var wipeMethod = await _context.WipeMethods.FirstOrDefaultAsync(wm => wm.Name == dto.WipeMethodName);
        if (wipeMethod == null)
        {
            return BadRequest("Wipe method not found.");  // Returnerer fejl, hvis slettemetoden ikke findes
        }

        // Opretter et nyt WipeJob objekt med de indkomne data
        var wipeJob = new WipeJob
        {
            StartTime = DateTime.SpecifyKind(dto.StartTime, DateTimeKind.Utc),  // Set to UTC for konsistens
            EndTime = DateTime.SpecifyKind(dto.EndTime, DateTimeKind.Utc),
            Status = dto.Status,
            DiskId = disk.DiskID,  // Refererer til den fundne disk
            WipeMethodId = wipeMethod.WipeMethodID,  // Refererer til den fundne slettemetode
            PerformedByUserId = user.Id  // Gemmer den nuværende bruger som ansvarlig
        };

        // Gemmer WipeJob i databasen
        await _context.WipeJobs.AddAsync(wipeJob);
        await _context.SaveChangesAsync();  // Gemmer ændringerne til databasen

        // Mapper den gemte WipeJob til DTO for at returnere relevant data til brugeren
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
            PerformedBy = user.UserName  // Returnerer navnet på den bruger, der udførte sletningen
        };

        return CreatedAtAction(nameof(GetWipeReports), new { id = result.WipeJobId }, result);  // Returnerer Created-status og dataen
    }

    // DELETE: api/WipeReports/{id}
    [Authorize]  // Kræver autorisation for at slette en wipe report
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteWipeReport(int id)
    {
        var wipeJob = await _context.WipeJobs
            .FirstOrDefaultAsync(wj => wj.WipeJobId == id);

        if (wipeJob == null)  // Tjekker om WipeJob findes
        {
            return NotFound();  // Returnerer NotFound hvis det ikke findes
        }

        // Fjerner WipeJob fra databasen
        _context.WipeJobs.Remove(wipeJob);
        await _context.SaveChangesAsync();  // Gemmer ændringerne

        return NoContent();  // Returnerer NoContent (204), som betyder sletningen lykkedes
    }
}
