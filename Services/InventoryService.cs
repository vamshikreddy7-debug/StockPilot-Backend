using Microsoft.EntityFrameworkCore;
using StockPilot.API.Data;
using StockPilot.API.DTOs;
using StockPilot.API.Models;

namespace StockPilot.API.Services;

public class InventoryService : IInventoryService
{
    private readonly ApplicationDbContext _context;

    public InventoryService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<InventoryItemDto>> GetAllItemsAsync(int userId)
    {
        var items = await _context.InventoryItems
            .Where(i => i.UserId == userId)
            .OrderBy(i => i.Name)
            .ToListAsync();

        return items.Select(MapToDto).ToList();
    }

    public async Task<InventoryItemDto?> GetItemByIdAsync(int id, int userId)
    {
        var item = await _context.InventoryItems
            .FirstOrDefaultAsync(i => i.Id == id && i.UserId == userId);

        return item != null ? MapToDto(item) : null;
    }

    public async Task<InventoryItemDto> CreateItemAsync(int userId, CreateInventoryItemRequest request)
    {
        var item = new InventoryItem
        {
            UserId = userId,
            Name = request.Name,
            SKU = request.SKU,
            Category = request.Category,
            Quantity = request.Quantity,
            LowStockThreshold = request.LowStockThreshold,
            CostPrice = request.CostPrice,
            SellingPrice = request.SellingPrice,
            Barcode = request.Barcode,
            Description = request.Description
        };

        _context.InventoryItems.Add(item);
        await _context.SaveChangesAsync();

        return MapToDto(item);
    }

    public async Task<InventoryItemDto?> UpdateItemAsync(int id, int userId, UpdateInventoryItemRequest request)
    {
        var item = await _context.InventoryItems
            .FirstOrDefaultAsync(i => i.Id == id && i.UserId == userId);

        if (item == null) return null;

        if (!string.IsNullOrEmpty(request.Name)) item.Name = request.Name;
        if (!string.IsNullOrEmpty(request.Category)) item.Category = request.Category;
        if (request.Quantity.HasValue) item.Quantity = request.Quantity.Value;
        if (request.LowStockThreshold.HasValue) item.LowStockThreshold = request.LowStockThreshold.Value;
        if (request.CostPrice.HasValue) item.CostPrice = request.CostPrice.Value;
        if (request.SellingPrice.HasValue) item.SellingPrice = request.SellingPrice.Value;
        if (request.Description != null) item.Description = request.Description;

        item.UpdatedAt = DateTime.UtcNow;
        _context.InventoryItems.Update(item);
        await _context.SaveChangesAsync();

        return MapToDto(item);
    }

    public async Task<bool> DeleteItemAsync(int id, int userId)
    {
        var item = await _context.InventoryItems
            .FirstOrDefaultAsync(i => i.Id == id && i.UserId == userId);

        if (item == null) return false;

        _context.InventoryItems.Remove(item);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<InventoryItemDto>> GetLowStockItemsAsync(int userId)
    {
        var items = await _context.InventoryItems
            .Where(i => i.UserId == userId && i.Quantity <= i.LowStockThreshold)
            .OrderBy(i => i.Quantity)
            .ToListAsync();

        return items.Select(MapToDto).ToList();
    }

    public async Task<List<InventoryItemDto>> SearchItemsAsync(int userId, string query)
    {
        var items = await _context.InventoryItems
            .Where(i => i.UserId == userId && 
                   (i.Name.Contains(query) || i.SKU.Contains(query) || i.Barcode == query))
            .ToListAsync();

        return items.Select(MapToDto).ToList();
    }

    private static InventoryItemDto MapToDto(InventoryItem item)
    {
        var margin = item.SellingPrice > 0 
            ? Math.Round(((item.SellingPrice - item.CostPrice) / item.SellingPrice) * 100, 2)
            : 0;

        return new InventoryItemDto
        {
            Id = item.Id,
            Name = item.Name,
            SKU = item.SKU,
            Category = item.Category,
            Quantity = item.Quantity,
            LowStockThreshold = item.LowStockThreshold,
            CostPrice = item.CostPrice,
            SellingPrice = item.SellingPrice,
            Barcode = item.Barcode,
            Margin = margin,
            IsLowStock = item.Quantity <= item.LowStockThreshold
        };
    }
}
