using EraZor.Data;
using EraZor.Interfaces;
using EraZor.Model;
using Microsoft.EntityFrameworkCore;

namespace EraZor.Interfaces;

public class WipeMethodService : IWipeMethodService
{
    private readonly DataContext _context;

    public WipeMethodService(DataContext context)
    {
        _context = context;
    }

    // Henter alle slettemetoder
    public async Task<IEnumerable<WipeMethod>> GetAllWipeMethodsAsync()
    {
        return await _context.WipeMethods.ToListAsync();
    }

    // Henter en slette-metode baseret på ID
    public async Task<WipeMethod?> GetWipeMethodByIdAsync(int id)
    {
        return await _context.WipeMethods.FindAsync(id);
    }

    // Sletter en slette-metode baseret på ID
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
