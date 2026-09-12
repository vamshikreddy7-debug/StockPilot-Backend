namespace StockPilot.API.DTOs;

public class CreateSaleRequest
{
    public List<SaleItemRequest> Items { get; set; } = new();
    public string? CustomerName { get; set; }
    public string PaymentMode { get; set; } = "cash";
}

public class SaleItemRequest
{
    public int InventoryItemId { get; set; }
    public int Quantity { get; set; }
}

public class SaleDto
{
    public int Id { get; set; }
    public string ReceiptNumber { get; set; } = string.Empty;
    public string? CustomerName { get; set; }
    public decimal TotalAmount { get; set; }
    public string PaymentMode { get; set; } = string.Empty;
    public DateTime SaleDate { get; set; }
    public List<SaleItemDto> Items { get; set; } = new();
}

public class SaleItemDto
{
    public int Id { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public string SKU { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
}
