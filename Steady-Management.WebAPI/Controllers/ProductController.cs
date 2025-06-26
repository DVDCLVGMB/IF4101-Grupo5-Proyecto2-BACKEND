using Microsoft.AspNetCore.Mvc;
using Steady_Management.Business;
using Steady_Management.Domain;
using Steady_Management.Api.Dtos;
using System.Collections.Generic;
using System.Linq;

namespace Steady_Management.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ProductBusiness productBusiness;

        public ProductController(ProductBusiness business)
        {
            productBusiness = business;
        }

        // GET api/product
        [HttpGet]
        public ActionResult<IEnumerable<ProductDto>> GetAll()
        {
            var dtos = productBusiness
                .GetAllProducts()
                .Select(p => MapToDto(p));
            return Ok(dtos);
        }

        // GET api/product/5
        [HttpGet("{id}")]
        public ActionResult<ProductDto> GetById(int id)
        {
            try
            {
                var product = productBusiness.GetProductById(id);
                return Ok(MapToDto(product));
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // POST api/product
        [HttpPost]
        public ActionResult<ProductDto> Create([FromBody] ProductDto dto)
        {
            // dto.ProductId se ignora en creación
            var toInsert = new Product(0, dto.CategoryId, dto.ProductName, dto.Price);
            var newId = productBusiness.CreateProduct(toInsert);

            var created = productBusiness.GetProductById(newId);
            var resultDto = MapToDto(created);

            return CreatedAtAction(nameof(GetById),
                                   new { id = resultDto.ProductId },
                                   resultDto);
        }

        // PUT api/product/5
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] ProductDto dto)
        {
            if (id != dto.ProductId)
                return BadRequest("El ID de ruta no coincide con el cuerpo.");

            var toUpdate = new Product(dto.ProductId, dto.CategoryId, dto.ProductName, dto.Price);
            try
            {
                productBusiness.UpdateProduct(toUpdate);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // DELETE api/product/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                productBusiness.DeleteProduct(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // Mapeo de dominio con DTO
        private static ProductDto MapToDto(Product p) => new ProductDto
        {
            ProductId = p.ProductId,
            CategoryId = p.CategoryId,
            ProductName = p.ProductName,
            Price = p.Price
        };
    }
}
