using Microsoft.AspNetCore.Mvc;
using Steady_Management.Api.Dtos;
using Steady_Management.Business;
using Steady_Management.Domain;
using System.Collections.Generic;
using System.Linq;

namespace Steady_Management.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryBusiness _business;

        public CategoryController(CategoryBusiness business)
        {
            _business = business;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CategoryDto>> GetAll()
        {
            var list = _business
                .GetAllCategories()
                .Select(c => new CategoryDto
                {
                    CategoryId = c.CategoryId,
                    Description = c.Description
                });
            return Ok(list);
        }

        [HttpGet("{id}")]
        public ActionResult<CategoryDto> GetById(int id)
        {
            try
            {
                var c = _business.GetCategoryById(id);
                return Ok(new CategoryDto
                {
                    CategoryId = c.CategoryId,
                    Description = c.Description
                });
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public ActionResult<CategoryDto> Create([FromBody] CategoryDto dto)
        {
            var toInsert = new Category(0, dto.Description);
            var newId = _business.CreateCategory(toInsert);
            var created = _business.GetCategoryById(newId);

            var result = new CategoryDto
            {
                CategoryId = created.CategoryId,
                Description = created.Description
            };

            return CreatedAtAction(nameof(GetById),
                                   new { id = newId },
                                   result);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] CategoryDto dto)
        {
            if (id != dto.CategoryId)
                return BadRequest("El ID de ruta no coincide con el del cuerpo");

            var toUpdate = new Category(dto.CategoryId, dto.Description);
            try
            {
                _business.UpdateCategory(toUpdate);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _business.DeleteCategory(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
