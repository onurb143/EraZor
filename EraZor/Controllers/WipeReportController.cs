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

        // GET: api/Report
        [HttpGet]
        public async Task<IActionResult> GetWipeReports()
        {
            var wipeReports = await _context.WipeJobs
                .Join(_context.WipeMethods,
                      wj => wj.WipeMethodId,
                      wm => wm.WipeMethodID,
                      (wj, wm) => new { wj, wm })
                .Join(_context.Disks,
                      wmw => wmw.wj.DiskId,
                      d => d.DiskID,
                      (wmw, d) => new WipeReport
                      {
                          WipeJobId = wmw.wj.WipeJobId,
                          StartTime = wmw.wj.StartTime,
                          EndTime = wmw.wj.EndTime,
                          Status = wmw.wj.Status,
                          DiskType = d.Type,
                          Capacity = d.Capacity,
                          SerialNumber = d.SerialNumber,
                          Manufacturer = d.Manufacturer,
                          WipeMethodName = wmw.wm.Name,
                          OverwritePasses = wmw.wm.OverwritePass
                      })
                .ToListAsync();

            return Ok(wipeReports);
        }


    }
}
