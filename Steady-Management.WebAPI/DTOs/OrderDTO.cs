using Steady_Management.Api.Dtos;
using System.ComponentModel.DataAnnotations;

namespace Steady_Management.WebAPI.DTOs

{
    public class OrderDTO
    {

        public OrderDTO()
        {
            OrderDetails = new List<OrderDetailDTO>();
            CreditCardNumber = string.Empty;
        }


        [Required]
        public int ClientId { get; set; }

        [Required]
        public int EmployeeId { get; set; }

        [Required]
        public int CityId { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "Debe incluir al menos un producto en la orden.")]
        public List<OrderDetailDTO> OrderDetails { get; set; } = new();

        [Required]
        public int PaymentMethodId { get; set; }

        public string PaymentMethodName { get; set; } = string.Empty;

        [Required]
        public DateTime PaymentDate { get; set; }

        public string CreditCardNumber { get; set; } = string.Empty;

      
        public decimal PaymentQuantity { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (PaymentMethodId == 2) // Tarjeta
            {
                if (string.IsNullOrWhiteSpace(CreditCardNumber) ||
                    CreditCardNumber.Length != 16 ||
                    !CreditCardNumber.All(char.IsDigit))
                {
                    yield return new ValidationResult("Número de tarjeta inválido.", new[] { nameof(CreditCardNumber) });
                }
            }
        }


    }
}
