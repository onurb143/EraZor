﻿using EraZor.Data;

using EraZor.Model;
using Microsoft.EntityFrameworkCore;


namespace EraZor.Interfaces
{
    public class DiskService : IDiskService
    {
        private readonly DataContext _context;

        public DiskService(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Disk>> GetDisksAsync()
        {
            return await _context.Disks.ToListAsync();
        }

        public async Task<Disk?> GetDiskByIdAsync(int id)
        {
            return await _context.Disks.FindAsync(id);
        }

        public async Task AddDiskAsync(Disk disk)
        {
            _context.Disks.Add(disk);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDiskAsync(int id)
        {
            var disk = await _context.Disks.FindAsync(id);
            if (disk != null)
            {
                _context.Disks.Remove(disk);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> DiskExistsAsync(int id)
        {
            return await _context.Disks.AnyAsync(d => d.DiskID == id);
        }
    }

}
