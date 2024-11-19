using Microsoft.AspNetCore.Mvc;
using EraZor.Data; // Din DbContext
using EraZor.Models; // Din LogEntry-model
using System.Linq;

namespace EraZor.Controllers
{
    [ApiController] // Marker controlleren som en API-controller
    [Route("api/[controller]")] // Definer baseruten som /api/LogEntry
    public class LogEntryController : ControllerBase
    {
        private readonly DataContext _context;

        public LogEntryController(DataContext context)
        {
            _context = context;
        }

        // GET: api/LogEntry
        [HttpGet]
        public IActionResult GetAllLogEntries()
        {
            var logs = _context.LogEntries.ToList(); // Hent alle log entries
            return Ok(logs);
        }

        // GET: api/LogEntry/5
        [HttpGet("{id}")]
        public IActionResult GetLogEntryById(int id)
        {
            var log = _context.LogEntries.FirstOrDefault(l => l.LogID == id);
            if (log == null) return NotFound();

            return Ok(log);
        }

        // POST: api/LogEntry
        [HttpPost]
        public IActionResult CreateLogEntry([FromBody] LogEntry logEntry)
        {
            if (ModelState.IsValid)
            {
                _context.LogEntries.Add(logEntry);
                _context.SaveChanges();
                return CreatedAtAction(nameof(GetLogEntryById), new { id = logEntry.LogID }, logEntry);
            }
            return BadRequest(ModelState);
        }

        // PUT: api/LogEntry/5
        [HttpPut("{id}")]
        public IActionResult UpdateLogEntry(int id, [FromBody] LogEntry logEntry)
        {
            if (id != logEntry.LogID)
            {
                return BadRequest("LogEntry ID mismatch.");
            }

            if (!_context.LogEntries.Any(l => l.LogID == id))
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.LogEntries.Update(logEntry);
                _context.SaveChanges();
                return NoContent(); // Returner 204 ved succes
            }
            return BadRequest(ModelState);
        }

        // DELETE: api/LogEntry/5
        [HttpDelete("{id}")]
        public IActionResult DeleteLogEntry(int id)
        {
            var log = _context.LogEntries.FirstOrDefault(l => l.LogID == id);
            if (log == null)
            {
                return NotFound();
            }

            _context.LogEntries.Remove(log);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
