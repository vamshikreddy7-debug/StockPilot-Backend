namespace StockPilot.API.Models;

public class SaleItem
{
    public int Id { get; set; }
    public int SaleId { get; set; }
    public int InventoryItemId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }

    // Navigation properties
    public Sale? Sale { get; set; }
    public InventoryItem? InventoryItem { get; set; }
}
