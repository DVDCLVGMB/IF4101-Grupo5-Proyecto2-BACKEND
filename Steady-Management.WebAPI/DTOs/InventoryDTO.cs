using System.Text.Json.Serialization;

public class InventoryDto
{
    public int InventoryId { get; set; }

    public int ProductId { get; set; }

    public int ItemQuantity { get; set; }

    public int LimitUntilRestock { get; set; }

    public string? Size { get; set; }

    // Este no lo devuelve la API, lo rellenas en tu View code-behind
    public string ProductName { get; set; } = string.Empty;
}
