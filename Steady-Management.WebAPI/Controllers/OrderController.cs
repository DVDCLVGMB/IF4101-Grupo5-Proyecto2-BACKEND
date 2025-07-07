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
        private readonly OrderBusiness _orderBusiness;

        // Un solo constructor con inyección de dependencias
        public OrderController(OrderBusiness orderBusiness)
        {
            _orderBusiness = orderBusiness;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderResponseDto>>> GetAll()
        {
            // Trae los pedidos con sus navegaciones
            var pedidos = _orderBusiness.GetAll();
            // Mapéalos a tu DTO de respuesta
            var result = pedidos.Select(o => new OrderResponseDto
            {
                OrderId = o.OrderId,
                ClientId = o.ClientId,      // usa la entidad Client
                EmployeeId = o.EmployeeId,   // usa la entidad Employee
                CityId = o.CityId,           // usa la entidad City
                OrderDate = o.OrderDate
            });
            return Ok(result);
        }

        [HttpGet("all")]  // Ruta específica: api/Order/all
        public ActionResult<List<OrderDTO>> GetAllOrders()
        {
            try
            {
                var rawData = _orderBusiness.GetAllOrders();
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

        [HttpGet("all-raw")]  // Ruta específica: api/Order/all-raw
        public ActionResult<List<Order>> GetAllOrderRaw()
        {
            var orders = _orderBusiness.GetAll();
            if (orders == null || !orders.Any())
            {
                return NotFound("No se encontraron órdenes.");
            }
            return Ok(orders);
        }

        [HttpGet("client/{clientId}")]
        public ActionResult<List<Order>> GetOrdersByClientId(int clientId)
        {
            var orders = _orderBusiness.GetByClientId(clientId);
            if (orders == null || !orders.Any())
            {
                return NotFound($"No se encontraron órdenes para el cliente con ID: {clientId}.");
            }
            return Ok(orders);
        }

        [HttpGet("city/{cityId}")]  // Añadido el atributo [HttpGet]
        public ActionResult<List<Order>> GetOrdersByCityId(int cityId)
        {
            var orders = _orderBusiness.GetByCityId(cityId);
            if (orders == null || !orders.Any())
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
                return BadRequest("Formato de fecha inválido. Use el formato YYYY-MM-DD.");
            }

            var orders = _orderBusiness.GetByDate(orderDate);
            if (orders == null || !orders.Any())
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
                var order = OrderMapper.ToOrder(dto);
                var orderDetails = OrderMapper.ToOrderDetails(dto);
                var payment = OrderMapper.ToPayment(dto);

                _orderBusiness.CreateOrder(order, orderDetails, payment);
                return Ok(new { message = "Orden creada correctamente." });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error en CreateOrder: " + ex.ToString()); // Para consola
                return BadRequest(new { error = ex.ToString() }); // Devuelve todo el stack trace para pruebas
            }
        }

        [HttpPut("{orderId}")]
        public IActionResult UpdateOrder(int orderId, [FromBody] OrderDTO dto)
        {
            try
            {
                var updatedOrder = OrderMapper.ToOrder(dto);
                var updatedDetails = OrderMapper.ToOrderDetails(dto);
                var updatedPayment = OrderMapper.ToPayment(dto);

                _orderBusiness.UpdateOrder(orderId, updatedOrder, updatedDetails, updatedPayment);
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
                _orderBusiness.DeleteOrder(orderId);
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

        [HttpGet("sales-tax")]
        public IActionResult GetSalesTaxPercentage()
        {
            try
            {
                decimal taxPercentage = _orderBusiness.GetSalesTaxPercentage();
                return Ok(taxPercentage);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = $"No se pudo obtener el impuesto: {ex.Message}" });
            }
        }
    }
}