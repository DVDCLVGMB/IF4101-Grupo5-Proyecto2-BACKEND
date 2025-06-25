
using Microsoft.AspNetCore.Mvc;
using SteadyManagement.Business;
using SteadyManagement.Domain;

namespace SteadyManagement.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly ClientBusiness _bl;
        public ClientsController(IConfiguration cfg)
        {
            _bl = new ClientBusiness(cfg.GetConnectionString("SteadyDB"));
        }

        [HttpGet] public IActionResult Get()               => Ok(_bl.GetAll());
        [HttpGet("{id:int}")]
        public IActionResult Get(int id)                   => _bl.GetById(id) is { } c ? Ok(c) : NotFound();
        [HttpPost]
        public IActionResult Post([FromBody] Client c)
        {
            _bl.AddClient(c);
            return CreatedAtAction(nameof(Get), new { id = c.ClientId }, c);
        }
        [HttpPut("{id:int}")]
        public IActionResult Put(int id, [FromBody] Client c)
        {
            if (id != c.ClientId) return BadRequest("ID mismatch");
            return _bl.Update(c) ? NoContent() : NotFound();
        }
        [HttpDelete("{id:int}")] public IActionResult Delete(int id) => _bl.Delete(id) ? NoContent() : NotFound();
    }
}
