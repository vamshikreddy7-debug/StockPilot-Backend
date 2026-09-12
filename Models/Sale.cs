namespace StockPilot.API.Models;

public class Sale
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string ReceiptNumber { get; set; } = string.Empty;
    public string? CustomerName { get; set; }
    public decimal TotalAmount { get; set; }
    public string PaymentMode { get; set; } = "cash"; // cash, upi, card
    public string Status { get; set; } = "paid";
    public DateTime SaleDate { get; set; } = DateTime.UtcNow;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public User? User { get; set; }
    public ICollection<SaleItem> SaleItems { get; set; } = new List<SaleItem>();
}
