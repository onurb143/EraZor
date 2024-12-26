/*using Microsoft.AspNetCore.Mvc;
using EraZor.Data;          // Din DbContext
using EraZor.Models;        // Din WipeJob-model
using EraZor.DTOs;          // DTO
using System.Linq;

namespace EraZor.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WipeJobController : ControllerBase
    {
        private readonly DataContext _context;

        public WipeJobController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAllWipeJobs()
        {
            var jobs = _context.WipeJobs.ToList();
            return Ok(jobs);
        }

        [HttpGet("{id}")]
        public IActionResult GetWipeJobById(int id)
        {
            var job = _context.WipeJobs.FirstOrDefault(w => w.WipeJobId == id);
            if (job == null) return NotFound();

            return Ok(job);
        }

        [HttpPost]
        public async Task<IActionResult> CreateWipeJob([FromBody] WipeJobCreateDto dto)
        {
            if (dto.StartTime.Kind != DateTimeKind.Utc || dto.EndTime.Kind != DateTimeKind.Utc)
            {
                return BadRequest("StartTime and EndTime must be in UTC format.");
            }

            var wipeJob = new WipeJob
            {
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                Status = dto.Status,
                DiskId = dto.DiskId,
                WipeMethodId = dto.WipeMethodId
            };

            _context.WipeJobs.Add(wipeJob);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetWipeJobById), new { id = wipeJob.WipeJobId }, wipeJob);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWipeJob(int id, [FromBody] WipeJob updatedWipeJob)
        {
            if (id != updatedWipeJob.WipeJobId) return BadRequest("ID mismatch.");

            var existingJob = await _context.WipeJobs.FindAsync(id);
            if (existingJob == null) return NotFound();

            existingJob.StartTime = updatedWipeJob.StartTime;
            existingJob.EndTime = updatedWipeJob.EndTime;
            existingJob.Status = updatedWipeJob.Status;
            existingJob.DiskId = updatedWipeJob.DiskId;
            existingJob.WipeMethodId = updatedWipeJob.WipeMethodId;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWipeJob(int id)
        {
            var job = await _context.WipeJobs.FindAsync(id);
            if (job == null) return NotFound();

            _context.WipeJobs.Remove(job);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
*/