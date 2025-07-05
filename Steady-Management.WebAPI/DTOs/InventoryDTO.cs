// Steady_Management.Api/Dtos/InventoryDto.cs
namespace Steady_Management.Api.Dtos
{
    public class InventoryDto
    {
        /// <summary>
        /// Identificador del producto al que pertenece este inventario.
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Talla del producto (S, M, L, XL, XXL). Puede ser nulo para productos sin talla.
        /// </summary>
        public string? Size { get; set; }

        /// <summary>
        /// Cantidad actual de unidades en stock.
        /// </summary>
        public int ItemQuantity { get; set; }

        /// <summary>
        /// Límite mínimo de existencias antes de necesitar reabastecer.
        /// </summary>
        public int LimitUntilRestock { get; set; }
    }
}
