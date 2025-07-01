using Microsoft.AspNetCore.Mvc;
using Steady_Management.Business;
using Steady_Management.WebAPI.DTOs;
using Steady_Management.WebAPI.Mappers;

namespace Steady_Management.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public OrderController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public IActionResult CreateOrder([FromBody] OrderDTO dto)
        {
            try
            { 
              
                string connectionString = _configuration.GetConnectionString("DefaultConnection")!;

                // Convertimos el DTO a entidades de dominio
                var order = OrderMapper.ToOrder(dto);
                var orderDetails = OrderMapper.ToOrderDetails(dto);
                var payment = OrderMapper.ToPayment(dto);

                // Creamos la orden
                var service = new OrderBusiness(connectionString);
                service.CreateOrder(order, orderDetails, payment);

                return Ok(new { message = "Orden creada correctamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}




