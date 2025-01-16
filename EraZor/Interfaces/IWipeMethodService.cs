using EraZor.Model;

namespace EraZor.Interfaces
{
    public interface IWipeMethodService
    {
        Task<IEnumerable<WipeMethod>> GetAllWipeMethodsAsync();
        Task<WipeMethod?> GetWipeMethodByIdAsync(int id);
        Task DeleteWipeMethodAsync(int id);
    }
}
