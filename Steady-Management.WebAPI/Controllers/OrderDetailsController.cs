using Microsoft.AspNetCore.Mvc;
using Steady_Management.Business;
using Steady_Management.Domain;
using System;
using System.Collections.Generic;

namespace Steady_Management.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderDetailsController : ControllerBase
    {
        private readonly OrderDetailsBusiness orderDetailBusiness;

        public OrderDetailsController(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            orderDetailBusiness = new OrderDetailsBusiness(connectionString);
        }

        // GET: api/orderDetails
        [HttpGet]
        public ActionResult<List<OrderDetail>> GetAll()
        {
            var orderDetails = orderDetailBusiness.GetAll();
            return Ok(orderDetails);
        }
    }
}