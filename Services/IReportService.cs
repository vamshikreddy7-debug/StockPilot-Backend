namespace StockPilot.API.Services;

public interface IReportService
{
    Task<ReportDto> GetDashboardReportAsync(int userId);
    Task<List<DailySalesDto>> GetDailySalesAsync(int userId, int days = 7);
    Task<List<TopItemsDto>> GetTopItemsAsync(int userId, int days = 30);
}

public class ReportDto
{
    public decimal TotalSalesToday { get; set; }
    public decimal TotalSalesThisWeek { get; set; }
    public int TotalItems { get; set; }
    public int LowStockCount { get; set; }
    public int PendingReceiptsCount { get; set; }
    public decimal PendingAmount { get; set; }
}

public class DailySalesDto
{
    public string Date { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public int TransactionCount { get; set; }
}

public class TopItemsDto
{
    public int ItemId { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public string SKU { get; set; } = string.Empty;
    public int QuantitySold { get; set; }
    public decimal Revenue { get; set; }
}
