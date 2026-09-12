using Microsoft.EntityFrameworkCore;
using StockPilot.API.Data;

namespace StockPilot.API.Services;

public class ReportService : IReportService
{
    private readonly ApplicationDbContext _context;

    public ReportService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ReportDto> GetDashboardReportAsync(int userId)
    {
        var today = DateTime.UtcNow.Date;
        var weekStart = today.AddDays(-(int)today.DayOfWeek);

        var totalSalesToday = await _context.Sales
            .Where(s => s.UserId == userId && s.SaleDate.Date == today)
            .SumAsync(s => s.TotalAmount);

        var totalSalesThisWeek = await _context.Sales
            .Where(s => s.UserId == userId && s.SaleDate >= weekStart)
            .SumAsync(s => s.TotalAmount);

        var totalItems = await _context.InventoryItems
            .CountAsync(i => i.UserId == userId);

        var lowStockCount = await _context.InventoryItems
            .CountAsync(i => i.UserId == userId && i.Quantity <= i.LowStockThreshold);

        return new ReportDto
        {
            TotalSalesToday = totalSalesToday,
            TotalSalesThisWeek = totalSalesThisWeek,
            TotalItems = totalItems,
            LowStockCount = lowStockCount
        };
    }

    public async Task<List<DailySalesDto>> GetDailySalesAsync(int userId, int days = 7)
    {
        var startDate = DateTime.UtcNow.AddDays(-days).Date;
        var sales = await _context.Sales
            .Where(s => s.UserId == userId && s.SaleDate >= startDate)
            .GroupBy(s => s.SaleDate.Date)
            .Select(g => new DailySalesDto
            {
                Date = g.Key.ToString("yyyy-MM-dd"),
                Amount = g.Sum(s => s.TotalAmount),
                TransactionCount = g.Count()
            })
            .OrderBy(d => d.Date)
            .ToListAsync();

        return sales;
    }

    public async Task<List<TopItemsDto>> GetTopItemsAsync(int userId, int days = 30)
    {
        var startDate = DateTime.UtcNow.AddDays(-days);
        var topItems = await _context.SaleItems
            .Where(si => si.Sale!.UserId == userId && si.Sale.SaleDate >= startDate)
            .GroupBy(si => new { si.InventoryItemId, si.InventoryItem!.Name, si.InventoryItem.SKU })
            .Select(g => new TopItemsDto
            {
                ItemId = g.Key.InventoryItemId,
                ItemName = g.Key.Name,
                SKU = g.Key.SKU,
                QuantitySold = g.Sum(si => si.Quantity),
                Revenue = g.Sum(si => si.TotalPrice)
            })
            .OrderByDescending(t => t.Revenue)
            .Take(10)
            .ToListAsync();

        return topItems;
    }
}
