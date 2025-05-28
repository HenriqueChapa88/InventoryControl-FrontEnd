using InventoryControl.Core.Entities;
using InventoryControl.Core.Interfaces;
using InventoryControl.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace InventoryControl.Infrastructure.Repositories
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly AppDbContext _context;

        public SupplierRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Supplier>> GetAllAsync()
        {
            return await _context.Suppliers
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Supplier?> GetByIdAsync(int id)
        {
            return await _context.Suppliers
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Supplier> AddAsync(Supplier entity)
        {
            await _context.Suppliers.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(Supplier entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Supplier entity)
        {
            _context.Suppliers.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> CNPJExistsAsync(string cnpj)
        {
            return await _context.Suppliers
                .AnyAsync(s => s.CNPJ == cnpj);
        }

        public async Task<bool> HasItemsAsync(int supplierId)
        {
            return await _context.Items
                .AnyAsync(i => i.SupplierId == supplierId);
        }
    }
}