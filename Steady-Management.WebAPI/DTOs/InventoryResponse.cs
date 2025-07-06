public class InventoryResponseDto
{
    public int InventoryId { get; set; }
    public int ProductId { get; set; }
    public int ItemQuantity { get; set; }
    public int LimitUntilRestock { get; set; }
    public string? Size { get; set; }
    public string ProductName { get; set; } = string.Empty;
}
