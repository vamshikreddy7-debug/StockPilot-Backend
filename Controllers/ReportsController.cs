using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockPilot.API.Services;
using System.Security.Claims;

namespace StockPilot.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReportsController : ControllerBase
{
    private readonly IReportService _reportService;

    public ReportsController(IReportService reportService)
    {
        _reportService = reportService;
    }

    private int GetUserId() => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

    [HttpGet("dashboard")]
    public async Task<ActionResult<ReportDto>> GetDashboardReport()
    {
        var userId = GetUserId();
        var report = await _reportService.GetDashboardReportAsync(userId);
        return Ok(report);
    }

    [HttpGet("daily-sales")]
    public async Task<ActionResult<List<DailySalesDto>>> GetDailySales([FromQuery] int days = 7)
    {
        var userId = GetUserId();
        var sales = await _reportService.GetDailySalesAsync(userId, days);
        return Ok(sales);
    }

    [HttpGet("top-items")]
    public async Task<ActionResult<List<TopItemsDto>>> GetTopItems([FromQuery] int days = 30)
    {
        var userId = GetUserId();
        var items = await _reportService.GetTopItemsAsync(userId, days);
        return Ok(items);
    }
}
