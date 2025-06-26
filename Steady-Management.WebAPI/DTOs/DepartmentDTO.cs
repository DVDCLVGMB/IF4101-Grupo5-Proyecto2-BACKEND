using System.ComponentModel.DataAnnotations;

namespace Steady_Management.WebAPI.DTOs
{
    public class DepartmentDto
    {
        public int DeptId { get; set; }
        public string DeptName { get; set; }
    }

    public class DepartmentCreateDto
    {
        [Required(ErrorMessage = "El nombre del departamento es obligatorio")]
        [MaxLength(25, ErrorMessage = "Máximo 25 caracteres")]
        public string DeptName { get; set; }
    }
}
