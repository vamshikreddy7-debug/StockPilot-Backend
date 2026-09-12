using StockPilot.API.DTOs;

namespace StockPilot.API.Services;

public interface ISalesService
{
    Task<SaleDto> CreateSaleAsync(int userId, CreateSaleRequest request);
    Task<List<SaleDto>> GetSalesHistoryAsync(int userId, int days = 30);
    Task<SaleDto?> GetSaleByIdAsync(int id, int userId);
    Task<decimal> GetTotalSalesAsync(int userId, DateTime startDate, DateTime endDate);
    Task<int> GetTotalItemsSoldAsync(int userId, DateTime startDate, DateTime endDate);
}
