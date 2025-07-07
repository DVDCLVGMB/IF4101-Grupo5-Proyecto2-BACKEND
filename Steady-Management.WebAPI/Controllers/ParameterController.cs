using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Steady_Management.Business;
using SteadyManagement.Domain;
using Microsoft.Extensions.Configuration;

namespace Steady_Management.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ParameterController : ControllerBase
    {
        private readonly ParameterBusiness _parameterBusiness;

        public ParameterController(IConfiguration config)
        {
            string conn = config.GetConnectionString("DefaultConnection");
            _parameterBusiness = new ParameterBusiness(conn);
        }

        [HttpGet]
        public ActionResult<Parameter> Get()
        {
            try
            {
                var parameter = _parameterBusiness.GetParameter();
                if (parameter == null)
                    return NotFound("No se encontraron parámetros de configuración.");

                return Ok(parameter);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener parámetros: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor al obtener los parámetros.");
            }
        }

        [HttpPut]
        public IActionResult Put([FromBody] Parameter parameterToUpdate)
        {
            if (parameterToUpdate == null)
            {
                return BadRequest("El objeto de parámetros no puede ser nulo.");
            }

            try
            {
                _parameterBusiness.UpdateParameter(parameterToUpdate);
                return NoContent();
            }
            catch (ArgumentException aex)
            {
                return BadRequest(aex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar parámetros: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor al actualizar los parámetros.");
            }
        }
    }
}
