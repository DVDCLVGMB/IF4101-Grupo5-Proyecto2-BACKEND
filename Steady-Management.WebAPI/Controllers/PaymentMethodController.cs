using Microsoft.AspNetCore.Mvc;
using Steady_Management.Business;
using Steady_Management.WebAPI.DTOs;

namespace Steady_Management.WebAPI.Controllers
{
    [ApiController]
    [Route("api/paymentmethod")]
    public class PaymentMethodController : ControllerBase
    {
        private readonly PaymentMethodBusiness _paymentMethodBusiness;

        public PaymentMethodController(PaymentMethodBusiness paymentMethodBusiness)
        {
            _paymentMethodBusiness = paymentMethodBusiness; // Corregido: asignar al campo con _
        }

        [HttpGet]
        public ActionResult<IEnumerable<PaymentMethodDTO>> Get()
        {
            var list = _paymentMethodBusiness.GetAll();

            var dtoList = list.Select(d => new PaymentMethodDTO
            {
                paymentMethodId = d.PaymentMethodId,
                paymentMethodName = d.PaymentMethodName
            });

            return Ok(dtoList);
        }
    }
}