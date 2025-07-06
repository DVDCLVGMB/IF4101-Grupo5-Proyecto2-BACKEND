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


        [HttpGet("{inventoryId:int}")]
        public ActionResult<InventoryResponseDto> GetById(int inventoryId)
        {
            var inv = _business.GetById(inventoryId);         
            var prodName = _productData
                .GetAll()
                .FirstOrDefault(p => p.ProductId == inv.ProductId)
                ?.ProductName
                ?? "<desconocido>";

            var dto = new InventoryResponseDto
            {
                InventoryId = inv.InventoryId,
                ProductId = inv.ProductId,
                ItemQuantity = inv.ItemQuantity,
                LimitUntilRestock = inv.LimitUntilRestock,
                Size = inv.Size,
                ProductName = prodName
            };

            return Ok(dto);
        }

        // POST api/inventory
        [HttpPost]
        public ActionResult<InventoryResponseDto> Create([FromBody] InventoryDto dto)
        {
            if (dto == null) return BadRequest();

            // 1) crear y obtener Inventory con su nuevo ID
            var toInsert = new Inventory(
                dto.InventoryId,
                dto.ProductId,
                dto.Size,
                dto.ItemQuantity,
                dto.LimitUntilRestock
            );
            var createdInv = _business.CreateInventory(toInsert);

            // 2) rellenar el nombre de producto
            var prodName = _productData
                               .GetAll()
                               .FirstOrDefault(p => p.ProductId == createdInv.ProductId)
                           ?.ProductName ?? "<desconocido>";

            // 3) armar DTO de respuesta
            var response = new InventoryResponseDto
            {
                InventoryId = createdInv.InventoryId,
                ProductId = createdInv.ProductId,
                ItemQuantity = createdInv.ItemQuantity,
                LimitUntilRestock = createdInv.LimitUntilRestock,
                Size = createdInv.Size,
                ProductName = prodName
            };

            // 4) devolver 201 con la ruta GET correcta
            return CreatedAtAction(
                nameof(GetById),                         // debe existir [HttpGet("{inventoryId:int}")]
                new { inventoryId = createdInv.InventoryId },
                response
            );
        }

        // PUT api/inventory/5
        [HttpPut("{inventoryId}")]
        public IActionResult Update(int inventoryId, [FromBody] InventoryDto dto)
        {
            if (inventoryId != dto.InventoryId)
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
        [HttpDelete("{inventoryId}")]
        public IActionResult Delete(int inventoryId)
        {
            try
            {
                _business.DeleteInventoryById(inventoryId);
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
