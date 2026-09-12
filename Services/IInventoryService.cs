using StockPilot.API.DTOs;

namespace StockPilot.API.Services;

public interface IInventoryService
{
    Task<List<InventoryItemDto>> GetAllItemsAsync(int userId);
    Task<InventoryItemDto?> GetItemByIdAsync(int id, int userId);
    Task<InventoryItemDto> CreateItemAsync(int userId, CreateInventoryItemRequest request);
    Task<InventoryItemDto?> UpdateItemAsync(int id, int userId, UpdateInventoryItemRequest request);
    Task<bool> DeleteItemAsync(int id, int userId);
    Task<List<InventoryItemDto>> GetLowStockItemsAsync(int userId);
    Task<List<InventoryItemDto>> SearchItemsAsync(int userId, string query);
}
