namespace Steady_Management.Api.Dtos
{
    public class ProductDto
    {
        public int ProductId { get; set; }        // Se ignora en POST
        public int CategoryId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public float Price { get; set; }
    }
}
