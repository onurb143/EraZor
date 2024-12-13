using EraZor.Data;
using EraZor.Interfaces;
using EraZor.DTO;
using Microsoft.EntityFrameworkCore;

namespace EraZor.Repositories
{
    public class WipeReportService : IWipeReportService
    {
        private readonly DataContext _context;

        public WipeReportService(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<WipeReportReadDto>> GetWipeReportsAsync()
        {
            var wipeReports = await _context.WipeJobs
                .Include(wj => wj.WipeMethod)
                .Include(wj => wj.Disk)
                .Include(wj => wj.PerformedByUser)
                .ToListAsync();

            return wipeReports.Select(wj => new WipeReportReadDto
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
                PerformedBy = wj.PerformedByUser.UserName
            });
        }

        public async Task<bool> CreateWipeReportAsync(WipeReportCreateDto dto, string userId)
        {
            var disk = await _context.Disks.FirstOrDefaultAsync(d => d.SerialNumber == dto.SerialNumber);
            if (disk == null) return false;

            var wipeMethod = await _context.WipeMethods.FirstOrDefaultAsync(wm => wm.Name == dto.WipeMethodName);
            if (wipeMethod == null) return false;

            var wipeJob = new WipeJob
            {
                StartTime = DateTime.SpecifyKind(dto.StartTime, DateTimeKind.Utc),
                EndTime = DateTime.SpecifyKind(dto.EndTime, DateTimeKind.Utc),
                Status = dto.Status,
                DiskId = disk.DiskID,
                WipeMethodId = wipeMethod.WipeMethodID,
                PerformedByUserId = userId
            };

            await _context.WipeJobs.AddAsync(wipeJob);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteWipeReportAsync(int id)
        {
            var wipeJob = await _context.WipeJobs.FindAsync(id);
            if (wipeJob == null) return false;

            _context.WipeJobs.Remove(wipeJob);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
 
