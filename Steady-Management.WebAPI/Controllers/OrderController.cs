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



        [HttpGet]
        public ActionResult<List<OrderDTO>> GetAllOrders()
        {
            try
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection")!;
                var service = new OrderBusiness(connectionString);

                var rawData = service.GetAllOrders();

                var dtos = rawData.Select(x =>
                    OrderMapper.ToOrderDto(x.Order, x.Details, x.Payment, x.PaymentMethodName)
                ).ToList();

                return Ok(dtos);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("{orderId}")]
        public IActionResult UpdateOrder(int orderId, [FromBody] OrderDTO dto)
        {
            try
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection")!;
                var service = new OrderBusiness(connectionString);

                var updatedOrder = OrderMapper.ToOrder(dto);
                var updatedDetails = OrderMapper.ToOrderDetails(dto);
                var updatedPayment = OrderMapper.ToPayment(dto);

                service.UpdateOrder(orderId, updatedOrder, updatedDetails, updatedPayment);

                return Ok(new { message = "Orden actualizada correctamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete("{orderId}")]
        public IActionResult DeleteOrder(int orderId)
        {
            try
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection")!;
                var service = new OrderBusiness(connectionString);

                service.DeleteOrder(orderId);

                return Ok(new { message = "Orden eliminada correctamente." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = $"No se pudo eliminar la orden: {ex.Message}" });
            }
        }

    }
}




