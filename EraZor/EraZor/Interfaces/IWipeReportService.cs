using EraZor.DTO;

namespace EraZor.Interfaces
{
    public interface IWipeReportService
    {
        Task<IEnumerable<WipeReportReadDto>> GetWipeReportsAsync();
        Task<bool> CreateWipeReportAsync(WipeReportCreateDto dto, string userId);
        Task<bool> DeleteWipeReportAsync(int id);
    }
}



