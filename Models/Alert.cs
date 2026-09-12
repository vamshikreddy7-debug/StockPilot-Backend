namespace StockPilot.API.Models;

public class Alert
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int InventoryItemId { get; set; }
    public string AlertType { get; set; } = string.Empty; // "LowStock", "OutOfStock"
    public string Message { get; set; } = string.Empty;
    public bool IsRead { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public User? User { get; set; }
    public InventoryItem? InventoryItem { get; set; }
}
