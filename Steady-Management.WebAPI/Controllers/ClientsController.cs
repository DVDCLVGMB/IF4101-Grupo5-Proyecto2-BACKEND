using Microsoft.AspNetCore.Mvc;
using Steady_Management.Business;
using SteadyManagement.Domain;
using System;
using System.Collections.Generic;

namespace Steady_Management.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientController : ControllerBase
    {
        private readonly ClientBusiness clientBusiness;

        public ClientController(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            clientBusiness = new ClientBusiness(connectionString);
        }

        // GET: api/client
        [HttpGet]
        public ActionResult<List<Client>> GetAll()
        {
            var clients = clientBusiness.GetAll();
            return Ok(clients);
        }

        // GET: api/client/5
        [HttpGet("{id}")]
        public ActionResult<Client> GetById(int id)
        {
            var client = clientBusiness.GetById(id);
            if (client == null)
                return NotFound();

            return Ok(client);
        }

        // POST: api/client
        [HttpPost]
        public ActionResult<Client> Create([FromBody] Client client)
        {
            try
            {
                clientBusiness.Create(client);
                return CreatedAtAction(nameof(GetById), new { id = client.ClientId }, client);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/client/5
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Client client)
        {
            if (id != client.ClientId)
                return BadRequest("ID del cliente no coincide con el objeto enviado.");

            try
            {
                var result = clientBusiness.Update(client);
                if (!result) return NotFound();
                return NoContent(); // 204
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/client/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var deleted = clientBusiness.Delete(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
