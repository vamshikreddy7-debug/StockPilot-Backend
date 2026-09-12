using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockPilot.API.DTOs;
using StockPilot.API.Services;
using System.Security.Claims;

namespace StockPilot.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SalesController : ControllerBase
{
    private readonly ISalesService _salesService;

    public SalesController(ISalesService salesService)
    {
        _salesService = salesService;
    }

    private int GetUserId() => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

    [HttpPost]
    public async Task<ActionResult<SaleDto>> CreateSale([FromBody] CreateSaleRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var userId = GetUserId();
            var sale = await _salesService.CreateSaleAsync(userId, request);
            return CreatedAtAction(nameof(GetSale), new { id = sale.Id }, sale);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SaleDto>> GetSale(int id)
    {
        var userId = GetUserId();
        var sale = await _salesService.GetSaleByIdAsync(id, userId);
        return sale != null ? Ok(sale) : NotFound();
    }

    [HttpGet("history")]
    public async Task<ActionResult<List<SaleDto>>> GetSalesHistory([FromQuery] int days = 30)
    {
        var userId = GetUserId();
        var sales = await _salesService.GetSalesHistoryAsync(userId, days);
        return Ok(sales);
    }

    [HttpGet("total-sales")]
    public async Task<ActionResult<object>> GetTotalSales([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        var userId = GetUserId();
        var total = await _salesService.GetTotalSalesAsync(userId, startDate, endDate);
        return Ok(new { totalSales = total });
    }

    [HttpGet("total-items-sold")]
    public async Task<ActionResult<object>> GetTotalItemsSold([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        var userId = GetUserId();
        var total = await _salesService.GetTotalItemsSoldAsync(userId, startDate, endDate);
        return Ok(new { totalItemsSold = total });
    }
}
