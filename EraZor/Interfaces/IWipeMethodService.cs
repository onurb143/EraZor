using EraZor.Model;

namespace EraZor.Interfaces
{
    public interface IWipeMethodService
    {
        Task<IEnumerable<WipeMethod>> GetAllWipeMethodsAsync();
        Task<WipeMethod?> GetWipeMethodByIdAsync(int id);
        Task AddWipeMethodAsync(WipeMethod wipeMethod);
        Task UpdateWipeMethodAsync(WipeMethod wipeMethod);
        Task DeleteWipeMethodAsync(int id);
    }
}
