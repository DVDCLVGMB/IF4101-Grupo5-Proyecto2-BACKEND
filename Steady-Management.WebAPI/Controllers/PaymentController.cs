using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using Steady_Management.Business;
using Steady_Management.Domain;

namespace Steady_Management.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly PaymentBusiness _paymentBusiness;
        private readonly string _connectionString;

        public PaymentController(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
            _paymentBusiness = new PaymentBusiness(_connectionString);
        }

        [HttpGet("order/{orderId}")]
        public ActionResult<Payment> GetByOrderId(int orderId)
        {
            var payment = _paymentBusiness.GetByOrderId(orderId);
            if (payment == null)
                return NotFound($"No se encontró el pago para la orden con ID {orderId}");

            return Ok(payment);
        }

        [HttpPost]
        public IActionResult InsertPayment([FromBody] Payment payment)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            using var transaction = connection.BeginTransaction();

            try
            {
                _paymentBusiness.Insert(payment, connection, transaction);
                transaction.Commit();
                return Ok(new { message = "Pago insertado correctamente." });
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return BadRequest(new { error = $"Error al insertar el pago: {ex.Message}" });
            }
        }

        [HttpDelete("order/{orderId}")]
        public IActionResult DeleteByOrderId(int orderId)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            using var transaction = connection.BeginTransaction();

            try
            {
                _paymentBusiness.DeleteByOrderId(orderId, connection, transaction);
                transaction.Commit();
                return Ok(new { message = "Pago eliminado correctamente." });
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return BadRequest(new { error = $"Error al eliminar el pago: {ex.Message}" });
            }
        }

        [HttpGet("methods")]
        public IActionResult GetPaymentMethodName([FromQuery] int methodId)
        {
            try
            {
                var name = _paymentBusiness.GetPaymentMethodName(methodId);
                return Ok(name);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
