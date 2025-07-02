using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Steady_Management.Business;
using SteadyManagement.Domain;
using System.Collections.Generic;

namespace Steady_Management.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CityController : ControllerBase
    {
        private readonly CityBusiness _biz;

        public CityController(IConfiguration config)
        {
            var cs = config.GetConnectionString("DefaultConnection");
            _biz = new CityBusiness(cs);
        }

        // GET: api/city
        [HttpGet]
        public ActionResult<List<City>> GetAll()
            => Ok(_biz.GetAll());

        // GET: api/city/5
        [HttpGet("{id:int}")]
        public ActionResult<City> GetById(int id)
        {
            var city = _biz.GetById(id);
            if (city == null) return NotFound();
            return Ok(city);
        }

        [HttpPost]
        public ActionResult<City> Create([FromBody] City city)
        {
            var createdCity = _biz.Create(city);
            // Ahora createdCity es un City, no un int
            return CreatedAtAction(
                nameof(GetById),
                new { id = createdCity.CityId },
                createdCity
            );
        }

        // PUT: api/city/5
        [HttpPut("{id:int}")]
        public IActionResult Update(int id, [FromBody] City city)
        {
            if (id != city.CityId)
                return BadRequest("El ID no coincide con el objeto enviado.");

            var updated = _biz.Update(city);
            if (!updated) return NotFound();
            return NoContent();
        }

        // DELETE: api/city/5
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var deleted = _biz.Delete(id);
                if (!deleted)
                    return NotFound();

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                // Esto un 400 con el mensaje de ex.Message 
                return BadRequest(new { error = ex.Message });
            }
        }

    }
}
