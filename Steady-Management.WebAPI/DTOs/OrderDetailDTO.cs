using System.ComponentModel.DataAnnotations;

namespace Steady_Management.Api.Dtos

{
    public class OrderDetailDTO
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser al menos 1.")]
        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }


    }
}
