using Microsoft.AspNetCore.Mvc;
using Steady_Management.Business;
using Steady_Management.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Steady_Management.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolesController : ControllerBase
    {
        private readonly RoleBusiness _roleBusiness;

        public RolesController(RoleBusiness roleBusiness)
        {
            _roleBusiness = roleBusiness;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Role>>> GetAllRoles()
        {
            var roles = await _roleBusiness.GetAllRolesAsync();
            return Ok(roles);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Role>> GetRoleById(int id)
        {
            var role = await _roleBusiness.GetRoleByIdAsync(id);

            if (role == null)
            {
                return NotFound();
            }

            return Ok(role);
        }

        [HttpPost]
        public async Task<ActionResult<int>> CreateRole([FromBody] Role role)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var roleId = await _roleBusiness.CreateRoleAsync(role);
            return CreatedAtAction(nameof(GetRoleById), new { id = roleId }, roleId);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRole(int id, [FromBody] Role role)
        {
            if (id != role.RoleId)
            {
                return BadRequest("ID mismatch");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingRole = await _roleBusiness.GetRoleByIdAsync(id);
            if (existingRole == null)
            {
                return NotFound();
            }

            await _roleBusiness.UpdateRoleAsync(role);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var role = await _roleBusiness.GetRoleByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            await _roleBusiness.DeleteRoleAsync(id);
            return NoContent();
        }
    }
}
