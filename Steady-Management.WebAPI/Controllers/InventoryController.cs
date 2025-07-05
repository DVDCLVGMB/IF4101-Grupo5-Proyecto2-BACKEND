// Steady_Management.Api/Controllers/InventoryController.cs

using Microsoft.AspNetCore.Mvc;
using Steady_Management.Api.Dtos;
using Steady_Management.Business;
using Steady_Management.DataAccess;
using Steady_Management.Domain;
using System.Collections.Generic;
using System.Linq;

namespace Steady_Management.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly InventoryBusiness _business;

        public InventoryController(InventoryBusiness business)
        {
            _business = business;
        }

        // GET api/inventory
        [HttpGet]
        public ActionResult<IEnumerable<InventoryDto>> GetAll()
        {
            var dtos = _business
                .GetAllInventories()
                .Select(inv => MapToDto(inv));
            return Ok(dtos);
        }

        // GET api/inventory/5
        [HttpGet("{productId}")]
        public ActionResult<InventoryDto> GetByProductId(int productId)
        {
            try
            {
                var inv = _business.GetInventoryByProductId(productId);
                return Ok(MapToDto(inv));
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // POST api/inventory
        [HttpPost]
        public ActionResult<InventoryDto> Create([FromBody] InventoryDto dto)
        {
            var toInsert = new Inventory(
                dto.ProductId,
                dto.Size,
                dto.ItemQuantity,
                dto.LimitUntilRestock
            );

            _business.CreateInventory(toInsert);

            // Devolver la entidad recién creada (sin ubicación concreta)
            var created = _business.GetInventoryByProductId(dto.ProductId);
            return CreatedAtAction(
                nameof(GetByProductId),
                new { productId = created.ProductId },
                MapToDto(created)
            );
        }

        // PUT api/inventory/5
        [HttpPut("{productId}")]
        public IActionResult Update(int productId, [FromBody] InventoryDto dto)
        {
            if (productId != dto.ProductId)
                return BadRequest("El ID de ruta no coincide con el cuerpo.");

            var toUpdate = new Inventory(
                dto.ProductId,
                dto.Size,
                dto.ItemQuantity,
                dto.LimitUntilRestock
            );

            try
            {
                _business.UpdateInventory(toUpdate);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // DELETE api/inventory/5
        [HttpDelete("{productId}")]
        public IActionResult Delete(int productId)
        {
            try
            {
                _business.DeleteInventory(productId);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // Mapea dominio ⇄ DTO
        private static InventoryDto MapToDto(Inventory inv) => new InventoryDto
        {
            ProductId = inv.ProductId,
            Size = inv.Size,
            ItemQuantity = inv.ItemQuantity,
            LimitUntilRestock = inv.LimitUntilRestock
        };
    }
}
