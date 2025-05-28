using InventoryControl.Core.Entities;
using InventoryControl.Core.Interfaces;
using InventoryControl.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace InventoryControl.Infrastructure.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private readonly AppDbContext _context;

        public ItemRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Item>> GetAllAsync()
        {
            return await _context.Items
                .Include(i => i.Supplier)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Item?> GetByIdAsync(int id)
        {
            return await _context.Items
                .Include(i => i.Supplier)
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<Item> AddAsync(Item entity)
        {
            await _context.Items.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(Item entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Item entity)
        {
            _context.Items.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Item>> GetBySupplierAsync(int supplierId)
        {
            return await _context.Items
                .Where(i => i.SupplierId == supplierId)
                .Include(i => i.Supplier)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Item?> GetBySkuAsync(string sku)
        {
            return await _context.Items
                .FirstOrDefaultAsync(i => i.SKU == sku);
        }
    }
}