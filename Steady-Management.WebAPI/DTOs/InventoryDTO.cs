using System.Text.Json.Serialization;

public class InventoryDto
{
    [JsonPropertyName("inventoryId")]
    public int InventoryId { get; set; }

    [JsonPropertyName("productId")]
    public int ProductId { get; set; }

    [JsonPropertyName("itemQuantity")]
    public int ItemQuantity { get; set; }

    [JsonPropertyName("limitUntilRestock")]
    public int LimitUntilRestock { get; set; }

    [JsonPropertyName("size")]
    public string? Size { get; set; }

    // Este no lo devuelve la API, lo rellenas en tu View code-behind
    public string ProductName { get; set; } = string.Empty;
}
