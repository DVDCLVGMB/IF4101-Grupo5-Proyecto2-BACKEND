using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Steady_Management.Api.Dtos;
using Steady_Management.Business;
using Steady_Management.Data;
using Steady_Management.DataAccess;
using Steady_Management.Domain;
using System.Collections.Generic;
using System.Linq;


namespace Steady_Management.Api.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly InventoryBusiness _business;
        private readonly InventoryData _inventoryData;
        private readonly ProductData _productData;

        // Inyecta aquí todas las dependencias que vayas a usar
        public InventoryController(
            InventoryBusiness business,
            InventoryData inventoryData,
            ProductData productData)
        {
            _business = business ?? throw new ArgumentNullException(nameof(business));
            _inventoryData = inventoryData ?? throw new ArgumentNullException(nameof(inventoryData));
            _productData = productData ?? throw new ArgumentNullException(nameof(productData));
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            // Ya no nulos
            var invs = _inventoryData.GetAll();   // List<InventoryEntity>
            var prods = _productData.GetAll();   // List<ProductEntity>

            var result = invs.Select(inv => new InventoryResponseDto
            {
                InventoryId = inv.InventoryId,
                ProductId = inv.ProductId,
                ItemQuantity = inv.ItemQuantity,
                LimitUntilRestock = inv.LimitUntilRestock,
                Size = inv.Size,
                ProductName = prods
                                        .FirstOrDefault(p => p.ProductId == inv.ProductId)
                                        ?.ProductName ?? "<desconocido>"
            }).ToList();

            return Ok(result);
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
                dto.InventoryId,
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
                dto.InventoryId,
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
            InventoryId = inv.InventoryId,
            ProductId = inv.ProductId,
            Size = inv.Size,
            ItemQuantity = inv.ItemQuantity,
            LimitUntilRestock = inv.LimitUntilRestock
        };

    }
}
