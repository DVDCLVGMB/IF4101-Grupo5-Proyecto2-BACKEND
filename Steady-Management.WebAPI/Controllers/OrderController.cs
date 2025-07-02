using Microsoft.AspNetCore.Mvc;
using Steady_Management.Business;
using Steady_Management.Domain;
using Steady_Management.WebAPI.DTOs;
using Steady_Management.WebAPI.Mappers;

namespace Steady_Management.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly OrderBusiness _orderBusiness;

        public OrderController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public OrderController(OrderBusiness orderBusiness)
        {
            _orderBusiness = orderBusiness;
        }

        [HttpGet]
        public ActionResult<List<Order>> GetAllOrder()
        {
            var orders = _orderBusiness.GetAll();
            if (orders == null || orders.Count == 0)
            {
                return NotFound("No se encontraron órdenes.");
            }
            return Ok(orders);
        }

        [HttpGet("client/{clientId}")]
        public ActionResult<List<Order>> GetOrdersByClientId(int clientId)
        {
            var orders = _orderBusiness.GetByClientId(clientId);
            if (orders == null || orders.Count == 0)
            {
                return NotFound($"No se encontraron órdenes para el cliente con ID: {clientId}.");
            }
            return Ok(orders);
        }

        public ActionResult<List<Order>> GetOrdersByCityId(int cityId)
        {
            var orders = _orderBusiness.GetByCityId(cityId);
            if (orders == null || orders.Count == 0)
            {
                return NotFound($"No se encontraron órdenes para la ciudad con ID: {cityId}.");
            }
            return Ok(orders);
        }

        [HttpGet("date/{dateString}")]
        public ActionResult<List<Order>> GetOrdersByDate(string dateString)
        {
            if (!DateOnly.TryParse(dateString, out DateOnly orderDate))
            {
                return BadRequest("Formato de fecha inválido. Por favor, use el formato YYYY-MM-DD.");
            }

            var orders = _orderBusiness.GetByDate(orderDate);
            if (orders == null || orders.Count == 0)
            {
                return NotFound($"No se encontraron órdenes para la fecha: {dateString}.");
            }
            return Ok(orders);
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




