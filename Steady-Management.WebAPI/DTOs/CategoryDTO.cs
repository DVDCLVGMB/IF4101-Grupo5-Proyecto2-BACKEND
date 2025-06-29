namespace Steady_Management.Api.Dtos
{
    public class CategoryDto
    {
        public int CategoryId { get; set; }    // Ignorado en POST
        public string Description { get; set; } = string.Empty;
    }
}
