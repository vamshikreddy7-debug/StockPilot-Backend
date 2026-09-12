namespace StockPilot.API.DTOs;

public class CreateInventoryItemRequest
{
    public string Name { get; set; } = string.Empty;
    public string SKU { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public int LowStockThreshold { get; set; }
    public decimal CostPrice { get; set; }
    public decimal SellingPrice { get; set; }
    public string? Barcode { get; set; }
    public string? Description { get; set; }
}

public class UpdateInventoryItemRequest
{
    public string? Name { get; set; }
    public string? Category { get; set; }
    public int? Quantity { get; set; }
    public int? LowStockThreshold { get; set; }
    public decimal? CostPrice { get; set; }
    public decimal? SellingPrice { get; set; }
    public string? Description { get; set; }
}

public class InventoryItemDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string SKU { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public int LowStockThreshold { get; set; }
    public decimal CostPrice { get; set; }
    public decimal SellingPrice { get; set; }
    public string? Barcode { get; set; }
    public decimal Margin { get; set; }
    public bool IsLowStock { get; set; }
}
