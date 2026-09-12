using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockPilot.API.DTOs;
using StockPilot.API.Services;
using System.Security.Claims;

namespace StockPilot.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class InventoryController : ControllerBase
{
    private readonly IInventoryService _inventoryService;

    public InventoryController(IInventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }

    private int GetUserId() => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

    [HttpGet]
    public async Task<ActionResult<List<InventoryItemDto>>> GetAllItems()
    {
        var userId = GetUserId();
        var items = await _inventoryService.GetAllItemsAsync(userId);
        return Ok(items);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<InventoryItemDto>> GetItem(int id)
    {
        var userId = GetUserId();
        var item = await _inventoryService.GetItemByIdAsync(id, userId);
        return item != null ? Ok(item) : NotFound();
    }

    [HttpPost]
    public async Task<ActionResult<InventoryItemDto>> CreateItem([FromBody] CreateInventoryItemRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userId = GetUserId();
        var item = await _inventoryService.CreateItemAsync(userId, request);
        return CreatedAtAction(nameof(GetItem), new { id = item.Id }, item);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<InventoryItemDto>> UpdateItem(int id, [FromBody] UpdateInventoryItemRequest request)
    {
        var userId = GetUserId();
        var item = await _inventoryService.UpdateItemAsync(id, userId, request);
        return item != null ? Ok(item) : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteItem(int id)
    {
        var userId = GetUserId();
        var success = await _inventoryService.DeleteItemAsync(id, userId);
        return success ? NoContent() : NotFound();
    }

    [HttpGet("low-stock")]
    public async Task<ActionResult<List<InventoryItemDto>>> GetLowStockItems()
    {
        var userId = GetUserId();
        var items = await _inventoryService.GetLowStockItemsAsync(userId);
        return Ok(items);
    }

    [HttpGet("search")]
    public async Task<ActionResult<List<InventoryItemDto>>> SearchItems([FromQuery] string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            return BadRequest("Search query is required");

        var userId = GetUserId();
        var items = await _inventoryService.SearchItemsAsync(userId, query);
        return Ok(items);
    }
}
