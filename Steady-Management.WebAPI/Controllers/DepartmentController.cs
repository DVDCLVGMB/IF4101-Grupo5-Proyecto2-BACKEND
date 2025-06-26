using Microsoft.AspNetCore.Mvc;
using Steady_Management.Business;
using Steady_Management.Domain;
using Steady_Management.WebAPI.DTOs;
using SteadyManagement.Business;

namespace Steady_Management.WebAPI.Controllers
{
    [ApiController]
    [Route("api/departments")]
    public class DepartmentController : ControllerBase
    {
        private readonly DepartmentBusiness _business;

        public DepartmentController(DepartmentBusiness business)
        {
            _business = business;
        }

        [HttpGet]
        public ActionResult<IEnumerable<DepartmentDto>> Get()
        {
            var list = _business.GetAll();

            var dtoList = list.Select(d => new DepartmentDto
            {
                DeptId = d.DeptId,
                DeptName = d.DeptName
            });

            return Ok(dtoList);
        }

        [HttpGet("{id}")]
        public ActionResult<DepartmentDto> GetById(int id)
        {
            var d = _business.GetById(id);
            if (d == null)
                return NotFound();

            return Ok(new DepartmentDto
            {
                DeptId = d.DeptId,
                DeptName = d.DeptName
            });
        }

        [HttpPost]
        public ActionResult Post([FromBody] DepartmentCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var department = new Department(0, dto.DeptName);
            _business.AddDepartment(department);

            return CreatedAtAction(nameof(GetById), new { id = department.DeptId }, null);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] DepartmentDto dto)
        {
            if (id != dto.DeptId)
                return BadRequest("ID no coincide");

            var existing = _business.GetById(id);
            if (existing == null)
                return NotFound();

            var updated = new Department(dto.DeptId, dto.DeptName);
            _business.Update(updated);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var existing = _business.GetById(id);
            if (existing == null)
                return NotFound();

            _business.Delete(id);
            return NoContent();
        }
    }
}

