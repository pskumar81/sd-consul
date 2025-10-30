using ElectronicShop.Shared.Models;
using ElectronicShop.InventoryService.Services;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicShop.InventoryService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InventoryController : ControllerBase
{
    private readonly IInventoryService _inventoryService;
    private readonly ILogger<InventoryController> _logger;

    public InventoryController(IInventoryService inventoryService, ILogger<InventoryController> logger)
    {
        _inventoryService = inventoryService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<InventoryItem>>>> GetAllInventory()
    {
        _logger.LogInformation("Getting all inventory items");
        var result = await _inventoryService.GetAllInventoryAsync();
        return Ok(result);
    }

    [HttpGet("product/{productId}")]
    public async Task<ActionResult<ApiResponse<InventoryItem?>>> GetInventoryByProductId(int productId)
    {
        _logger.LogInformation("Getting inventory for product ID: {ProductId}", productId);
        var result = await _inventoryService.GetInventoryByProductIdAsync(productId);
        
        if (!result.Success)
        {
            return NotFound(result);
        }
        
        return Ok(result);
    }

    [HttpPost("update-stock")]
    public async Task<ActionResult<ApiResponse<bool>>> UpdateStock(UpdateInventoryDto updateInventoryDto)
    {
        _logger.LogInformation("Updating stock for product ID: {ProductId}", updateInventoryDto.ProductId);
        var result = await _inventoryService.UpdateStockAsync(updateInventoryDto);
        
        if (!result.Success)
        {
            return BadRequest(result);
        }
        
        return Ok(result);
    }

    [HttpGet("check-availability")]
    public async Task<ActionResult<ApiResponse<bool>>> CheckStockAvailability(int productId, int quantity)
    {
        _logger.LogInformation("Checking stock availability for product ID: {ProductId}, quantity: {Quantity}", productId, quantity);
        var result = await _inventoryService.CheckStockAvailabilityAsync(productId, quantity);
        return Ok(result);
    }
}