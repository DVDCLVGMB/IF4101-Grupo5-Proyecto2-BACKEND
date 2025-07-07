using System.ComponentModel.DataAnnotations;

namespace Steady_Management.WebAPI.DTOs
{
    public class PaymentMethodDTO
    {
        public int paymentMethodId { get; set; }
        public string paymentMethodName { get; set; }
    }

    public class PaymentMethodCreateDto
    {
        [Required(ErrorMessage = "El nombre del método es obligatorio")]
        [MaxLength(25, ErrorMessage = "Máximo 25 caracteres")]
        public string paymentMethodName { get; set; }
    }
}
