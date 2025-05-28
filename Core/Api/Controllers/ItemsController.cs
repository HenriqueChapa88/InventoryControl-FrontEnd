using InventoryControl.Application.DTOs;
using InventoryControl.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryControl.Api.Controllers
{
    [Route("api/[controller]"), ApiController, Authorize]
    public class ItemsController : ControllerBase
    {
        private readonly IInventoryService _service;

        public ItemsController(IInventoryService service) => _service = service;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemDto>>> Get() => 
            Ok(await _service.GetAllItemsAsync());

        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> Get(int id)
        {
            try
            {
                return Ok(await _service.GetItemByIdAsync(id));
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost, Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<ItemDto>> Post([FromBody] CreateItemDto dto)
        {
            try
            {
                var item = await _service.AddItemAsync(dto);
                return CreatedAtAction(nameof(Get), new { id = item.Id }, item);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}"), Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Put(int id, [FromBody] UpdateItemDto dto)
        {
            try
            {
                await _service.UpdateItemAsync(id, dto);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _service.DeleteItemAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost("{id}/add-stock"), Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<ItemDto>> AddStock(int id, [FromBody] int quantity)
        {
            try
            {
                return Ok(await _service.AddStockAsync(id, quantity));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("low-stock/{threshold}"), Authorize]
        public async Task<ActionResult<IEnumerable<ItemDto>>> GetLowStock(int threshold) => 
            Ok(await _service.GetLowStockItemsAsync(threshold));

        [HttpGet("inventory-value"), Authorize]
        public async Task<ActionResult<decimal>> GetInventoryValue() => 
            Ok(await _service.CalculateInventoryValueAsync());
    }
}