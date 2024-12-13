using EraZor.Model;

namespace EraZor.Interfaces
{
    public interface IDiskService
    {
        Task<IEnumerable<Disk>> GetDisksAsync();
        Task<Disk?> GetDiskByIdAsync(int id);
        Task AddDiskAsync(Disk disk);
        Task DeleteDiskAsync(int id);
        Task<bool> DiskExistsAsync(int id);  // Tilføj denne metode som asynkron
    }
}
