using Microsoft.AspNetCore.Mvc;
using EraZor.Data; // Din DbContext
using EraZor.Models; // Din WipeJob-model
using System.Linq;

namespace EraZor.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // Definerer ruten som /api/WipeJob
    public class WipeJobController : ControllerBase
    {
        private readonly DataContext _context;

        public WipeJobController(DataContext context)
        {
            _context = context;
        }

        // GET: api/WipeJob
        [HttpGet]
        public IActionResult GetAllWipeJobs()
        {
            var jobs = _context.WipeJobs.ToList(); // Hent alle wipe jobs
            return Ok(jobs);
        }

        // GET: api/WipeJob/{id}
        [HttpGet("{id}")]
        public IActionResult GetWipeJobById(int id)
        {
            var job = _context.WipeJobs.FirstOrDefault(w => w.WipeJobId == id);
            if (job == null) return NotFound();

            return Ok(job);
        }

        // POST: api/WipeJob
        [HttpPost]
        public IActionResult CreateWipeJob([FromBody] WipeJob wipeJob)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _context.WipeJobs.Add(wipeJob);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetWipeJobById), new { id = wipeJob.WipeJobId }, wipeJob);
        }

        // PUT: api/WipeJob/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateWipeJob(int id, [FromBody] WipeJob updatedWipeJob)
        {
            if (id != updatedWipeJob.WipeJobId) return BadRequest("ID mismatch.");

            var existingJob = _context.WipeJobs.FirstOrDefault(w => w.WipeJobId == id);
            if (existingJob == null) return NotFound();

            existingJob.StartTime = updatedWipeJob.StartTime;
            existingJob.EndTime = updatedWipeJob.EndTime;
            existingJob.Status = updatedWipeJob.Status;
            existingJob.DiskId = updatedWipeJob.DiskId;
            existingJob.WipeMethodId = updatedWipeJob.WipeMethodId;
            existingJob.UserId = updatedWipeJob.UserId;

            _context.SaveChanges();

            return NoContent();
        }

        // DELETE: api/WipeJob/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteWipeJob(int id)
        {
            var job = _context.WipeJobs.FirstOrDefault(w => w.WipeJobId == id);
            if (job == null) return NotFound();

            _context.WipeJobs.Remove(job);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
