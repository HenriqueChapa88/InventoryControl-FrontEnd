using AutoMapper;
using InventoryControl.Core.Entities;
using InventoryControl.Core.Exceptions;
using InventoryControl.Core.Interfaces;
using InventoryControl.Application.DTOs;

namespace InventoryControl.Infrastructure.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IItemRepository _itemRepository;
        private readonly ISupplierRepository _supplierRepository;
        private readonly IMapper _mapper;

        public InventoryService(
            IItemRepository itemRepository,
            ISupplierRepository supplierRepository,
            IMapper mapper)
        {
            _itemRepository = itemRepository;
            _supplierRepository = supplierRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SupplierDto>> GetAllSuppliersAsync()
        {
            var suppliers = await _supplierRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<SupplierDto>>(suppliers);
        }

        public async Task<SupplierDto> GetSupplierByIdAsync(int id)
        {
            var supplier = await _supplierRepository.GetByIdAsync(id) 
                ?? throw new NotFoundException("Fornecedor não encontrado");
            return _mapper.Map<SupplierDto>(supplier);
        }

        public async Task<SupplierDto> AddSupplierAsync(CreateSupplierDto supplierDto)
        {
            if (await _supplierRepository.CNPJExistsAsync(supplierDto.CNPJ))
                throw new DomainException("CNPJ já cadastrado");

            var supplier = _mapper.Map<Supplier>(supplierDto);
            await _supplierRepository.AddAsync(supplier);
            return _mapper.Map<SupplierDto>(supplier);
        }

        public async Task UpdateSupplierAsync(int id, UpdateSupplierDto supplierDto)
        {
            var supplier = await _supplierRepository.GetByIdAsync(id) 
                ?? throw new NotFoundException("Fornecedor não encontrado");

            _mapper.Map(supplierDto, supplier);
            await _supplierRepository.UpdateAsync(supplier);
        }

        public async Task DeleteSupplierAsync(int id)
        {
            var supplier = await _supplierRepository.GetByIdAsync(id) 
                ?? throw new NotFoundException("Fornecedor não encontrado");

            if (await _itemRepository.GetBySupplierAsync(id) is var items && items.Any())
                throw new DomainException("Não é possível excluir fornecedor com itens associados");

            await _supplierRepository.DeleteAsync(supplier);
        }

        public async Task<IEnumerable<ItemDto>> GetItemsBySupplierAsync(int supplierId)
        {
            var items = await _itemRepository.GetBySupplierAsync(supplierId);
            return _mapper.Map<IEnumerable<ItemDto>>(items);
        }

        public async Task<IEnumerable<ItemDto>> GetAllItemsAsync()
        {
            var items = await _itemRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ItemDto>>(items);
        }

        public async Task<ItemDto> GetItemByIdAsync(int id)
        {
            var item = await _itemRepository.GetByIdAsync(id) 
                ?? throw new NotFoundException("Item não encontrado");
            return _mapper.Map<ItemDto>(item);
        }

        public async Task<IEnumerable<ItemDto>> GetItemsBySupplierIdAsync(int supplierId)
        {
            var items = await _itemRepository.GetBySupplierAsync(supplierId);
            return _mapper.Map<IEnumerable<ItemDto>>(items);
        }

        public async Task<ItemDto> AddItemAsync(CreateItemDto itemDto)
        {
            if (await _itemRepository.GetBySkuAsync(itemDto.SKU) != null)
                throw new DomainException("SKU já existe");

            var item = _mapper.Map<Item>(itemDto);
            await _itemRepository.AddAsync(item);
            return _mapper.Map<ItemDto>(item);
        }

        public async Task UpdateItemAsync(int id, UpdateItemDto itemDto)
        {
            var item = await _itemRepository.GetByIdAsync(id) 
                ?? throw new NotFoundException("Item não encontrado");

            _mapper.Map(itemDto, item);
            await _itemRepository.UpdateAsync(item);
        }

        public async Task DeleteItemAsync(int id)
        {
            var item = await _itemRepository.GetByIdAsync(id) 
                ?? throw new NotFoundException("Item não encontrado");

            await _itemRepository.DeleteAsync(item);
        }

        public async Task<decimal> CalculateInventoryValueAsync()
        {
            var items = await _itemRepository.GetAllAsync();
            return items.Sum(i => i.Quantity * i.UnitPrice);
        }

        public async Task<ItemDto> AddStockAsync(int itemId, int quantity)
        {
            if (quantity <= 0)
                throw new DomainException("Quantidade deve ser positiva");

            var item = await _itemRepository.GetByIdAsync(itemId) 
                ?? throw new NotFoundException("Item não encontrado");

            item.Quantity += quantity;
            await _itemRepository.UpdateAsync(item);
            return _mapper.Map<ItemDto>(item);
        }

        public async Task<ItemDto> RemoveStockAsync(int itemId, int quantity)
        {
            if (quantity <= 0)
                throw new DomainException("Quantidade deve ser positiva");

            var item = await _itemRepository.GetByIdAsync(itemId) 
                ?? throw new NotFoundException("Item não encontrado");

            if (item.Quantity < quantity)
                throw new DomainException("Estoque insuficiente");

            item.Quantity -= quantity;
            await _itemRepository.UpdateAsync(item);
            return _mapper.Map<ItemDto>(item);
        }

        public async Task<IEnumerable<ItemDto>> GetLowStockItemsAsync(int threshold)
        {
            var items = await _itemRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ItemDto>>(
                items.Where(i => i.Quantity < threshold));
        }
    }
}