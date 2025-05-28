using InventoryControl.Application.DTOs;
using InventoryControl.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryControl.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SuppliersController : ControllerBase
    {
        private readonly IInventoryService _service;

        public SuppliersController(IInventoryService service) => _service = service;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SupplierDto>>> Get() => 
            Ok(await _service.GetAllSuppliersAsync());

        [HttpGet("{id}")]
        public async Task<ActionResult<SupplierDto>> Get(int id)
        {
            try
            {
                return Ok(await _service.GetSupplierByIdAsync(id));
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<SupplierDto>> Post([FromBody] CreateSupplierDto dto)
        {
            try
            {
                var supplier = await _service.AddSupplierAsync(dto);
                return CreatedAtAction(nameof(Get), new { id = supplier.Id }, supplier);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Put(int id, [FromBody] UpdateSupplierDto dto)
        {
            try
            {
                await _service.UpdateSupplierAsync(id, dto);
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

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _service.DeleteSupplierAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("{id}/items")]
        public async Task<ActionResult<IEnumerable<ItemDto>>> GetItems(int id)
        {
            try
            {
                return Ok(await _service.GetItemsBySupplierAsync(id));
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}