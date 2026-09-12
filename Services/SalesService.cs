using Microsoft.EntityFrameworkCore;
using StockPilot.API.Data;
using StockPilot.API.DTOs;
using StockPilot.API.Models;

namespace StockPilot.API.Services;

public class SalesService : ISalesService
{
    private readonly ApplicationDbContext _context;

    public SalesService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SaleDto> CreateSaleAsync(int userId, CreateSaleRequest request)
    {
        // Verify all items exist and belong to user
        var itemIds = request.Items.Select(i => i.InventoryItemId).ToList();
        var inventoryItems = await _context.InventoryItems
            .Where(i => i.UserId == userId && itemIds.Contains(i.Id))
            .ToDictionaryAsync(i => i.Id);

        if (inventoryItems.Count != request.Items.Count)
            throw new ArgumentException("One or more items not found");

        var sale = new Sale
        {
            UserId = userId,
            ReceiptNumber = $"RCP-{DateTime.UtcNow:yyyyMMddHHmmss}",
            CustomerName = request.CustomerName,
            PaymentMode = request.PaymentMode,
            Status = "paid"
        };

        decimal totalAmount = 0;
        var saleItems = new List<SaleItem>();

        foreach (var item in request.Items)
        {
            var inventoryItem = inventoryItems[item.InventoryItemId];
            
            if (inventoryItem.Quantity < item.Quantity)
                throw new ArgumentException($"Insufficient stock for {inventoryItem.Name}");

            var saleItem = new SaleItem
            {
                InventoryItemId = item.InventoryItemId,
                Quantity = item.Quantity,
                UnitPrice = inventoryItem.SellingPrice,
                TotalPrice = inventoryItem.SellingPrice * item.Quantity
            };

            saleItems.Add(saleItem);
            totalAmount += saleItem.TotalPrice;

            // Update inventory
            inventoryItem.Quantity -= item.Quantity;
            _context.InventoryItems.Update(inventoryItem);

            // Create alert if low stock
            if (inventoryItem.Quantity <= inventoryItem.LowStockThreshold)
            {
                var alert = new Alert
                {
                    UserId = userId,
                    InventoryItemId = inventoryItem.Id,
                    AlertType = inventoryItem.Quantity == 0 ? "OutOfStock" : "LowStock",
                    Message = $"{inventoryItem.Name} is now {(inventoryItem.Quantity == 0 ? "out of stock" : "low on stock")}"
                };
                _context.Alerts.Add(alert);
            }
        }

        sale.TotalAmount = totalAmount;
        sale.SaleItems = saleItems;

        _context.Sales.Add(sale);
        await _context.SaveChangesAsync();

        return MapToDto(sale);
    }

    public async Task<List<SaleDto>> GetSalesHistoryAsync(int userId, int days = 30)
    {
        var startDate = DateTime.UtcNow.AddDays(-days);
        var sales = await _context.Sales
            .Where(s => s.UserId == userId && s.SaleDate >= startDate)
            .Include(s => s.SaleItems)
            .ThenInclude(si => si.InventoryItem)
            .OrderByDescending(s => s.SaleDate)
            .ToListAsync();

        return sales.Select(MapToDto).ToList();
    }

    public async Task<SaleDto?> GetSaleByIdAsync(int id, int userId)
    {
        var sale = await _context.Sales
            .Include(s => s.SaleItems)
            .ThenInclude(si => si.InventoryItem)
            .FirstOrDefaultAsync(s => s.Id == id && s.UserId == userId);

        return sale != null ? MapToDto(sale) : null;
    }

    public async Task<decimal> GetTotalSalesAsync(int userId, DateTime startDate, DateTime endDate)
    {
        return await _context.Sales
            .Where(s => s.UserId == userId && s.SaleDate >= startDate && s.SaleDate <= endDate)
            .SumAsync(s => s.TotalAmount);
    }

    public async Task<int> GetTotalItemsSoldAsync(int userId, DateTime startDate, DateTime endDate)
    {
        return await _context.SaleItems
            .Where(si => si.Sale!.UserId == userId && si.Sale.SaleDate >= startDate && si.Sale.SaleDate <= endDate)
            .SumAsync(si => si.Quantity);
    }

    private static SaleDto MapToDto(Sale sale)
    {
        return new SaleDto
        {
            Id = sale.Id,
            ReceiptNumber = sale.ReceiptNumber,
            CustomerName = sale.CustomerName,
            TotalAmount = sale.TotalAmount,
            PaymentMode = sale.PaymentMode,
            SaleDate = sale.SaleDate,
            Items = sale.SaleItems.Select(si => new SaleItemDto
            {
                Id = si.Id,
                ItemName = si.InventoryItem?.Name ?? "Unknown",
                SKU = si.InventoryItem?.SKU ?? "",
                Quantity = si.Quantity,
                UnitPrice = si.UnitPrice,
                TotalPrice = si.TotalPrice
            }).ToList()
        };
    }
}
