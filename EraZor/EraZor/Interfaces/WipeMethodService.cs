using EraZor.Data;
using EraZor.Model;
using Microsoft.EntityFrameworkCore;

namespace EraZor.Interfaces
{
    public class WipeMethodService : IWipeMethodService
    {
        private readonly DataContext _context;

        public WipeMethodService(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<WipeMethod>> GetAllWipeMethodsAsync()
        {
            return await _context.WipeMethods.ToListAsync();
        }

        public async Task<WipeMethod?> GetWipeMethodByIdAsync(int id)
        {
            return await _context.WipeMethods.FindAsync(id);
        }

        public async Task AddWipeMethodAsync(WipeMethod wipeMethod)
        {
            _context.WipeMethods.Add(wipeMethod);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateWipeMethodAsync(WipeMethod wipeMethod)
        {
            var existingWipeMethod = await _context.WipeMethods.FindAsync(wipeMethod.WipeMethodID);
            if (existingWipeMethod != null)
            {
                // Opdaterer de relevante felter
                existingWipeMethod.Name = wipeMethod.Name;
                existingWipeMethod.OverwritePass = wipeMethod.OverwritePass;
                existingWipeMethod.Description = wipeMethod.Description;

                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteWipeMethodAsync(int id)
        {
            var wipeMethod = await _context.WipeMethods.FindAsync(id);
            if (wipeMethod != null)
            {
                _context.WipeMethods.Remove(wipeMethod);
                await _context.SaveChangesAsync();
            }
        }
    }
}
