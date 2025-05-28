using InventoryControl.Application.DTOs;

namespace InventoryControl.Application.Interfaces
{
    public interface IInventoryService
    {
        Task<IEnumerable<SupplierDto>> GetAllSuppliersAsync();
        Task<SupplierDto> GetSupplierByIdAsync(int id);
        Task<SupplierDto> AddSupplierAsync(CreateSupplierDto supplierDto);
        Task UpdateSupplierAsync(int id, UpdateSupplierDto supplierDto);
        Task DeleteSupplierAsync(int id);
        Task<IEnumerable<ItemDto>> GetItemsBySupplierAsync(int supplierId);

        Task<IEnumerable<ItemDto>> GetAllItemsAsync();
        Task<ItemDto> GetItemByIdAsync(int id);
        Task<IEnumerable<ItemDto>> GetItemsBySupplierIdAsync(int supplierId);
        Task<ItemDto> AddItemAsync(CreateItemDto itemDto);
        Task UpdateItemAsync(int id, UpdateItemDto itemDto);
        Task DeleteItemAsync(int id);
        
        Task<decimal> CalculateInventoryValueAsync();
        Task<ItemDto> AddStockAsync(int itemId, int quantity);
        Task<ItemDto> RemoveStockAsync(int itemId, int quantity);
        Task<IEnumerable<ItemDto>> GetLowStockItemsAsync(int threshold);
    }
}