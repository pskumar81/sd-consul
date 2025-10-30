using ElectronicShop.Shared.Models;

namespace ElectronicShop.InventoryService.Services;

public interface IInventoryService
{
    Task<ApiResponse<IEnumerable<InventoryItem>>> GetAllInventoryAsync();
    Task<ApiResponse<InventoryItem?>> GetInventoryByProductIdAsync(int productId);
    Task<ApiResponse<bool>> UpdateStockAsync(UpdateInventoryDto updateInventoryDto);
    Task<ApiResponse<bool>> CheckStockAvailabilityAsync(int productId, int quantity);
}

public class InventoryService : IInventoryService
{
    private static readonly List<InventoryItem> _inventory = new()
    {
        new InventoryItem
        {
            Id = 1,
            ProductId = 1,
            QuantityInStock = 50,
            ReorderLevel = 10,
            MaxStockLevel = 100,
            LastUpdated = DateTime.UtcNow.AddDays(-5),
            StockMovements = new List<StockMovement>()
        },
        new InventoryItem
        {
            Id = 2,
            ProductId = 2,
            QuantityInStock = 75,
            ReorderLevel = 15,
            MaxStockLevel = 150,
            LastUpdated = DateTime.UtcNow.AddDays(-3),
            StockMovements = new List<StockMovement>()
        },
        new InventoryItem
        {
            Id = 3,
            ProductId = 3,
            QuantityInStock = 25,
            ReorderLevel = 5,
            MaxStockLevel = 50,
            LastUpdated = DateTime.UtcNow.AddDays(-2),
            StockMovements = new List<StockMovement>()
        },
        new InventoryItem
        {
            Id = 4,
            ProductId = 4,
            QuantityInStock = 100,
            ReorderLevel = 20,
            MaxStockLevel = 200,
            LastUpdated = DateTime.UtcNow.AddDays(-1),
            StockMovements = new List<StockMovement>()
        }
    };

    private static int _nextMovementId = 1;

    public Task<ApiResponse<IEnumerable<InventoryItem>>> GetAllInventoryAsync()
    {
        return Task.FromResult(new ApiResponse<IEnumerable<InventoryItem>>
        {
            Success = true,
            Data = _inventory,
            Message = $"Retrieved {_inventory.Count} inventory items"
        });
    }

    public Task<ApiResponse<InventoryItem?>> GetInventoryByProductIdAsync(int productId)
    {
        var inventoryItem = _inventory.FirstOrDefault(i => i.ProductId == productId);
        
        if (inventoryItem == null)
        {
            return Task.FromResult(new ApiResponse<InventoryItem?>
            {
                Success = false,
                Data = null,
                Message = "Inventory item not found for this product"
            });
        }

        return Task.FromResult(new ApiResponse<InventoryItem?>
        {
            Success = true,
            Data = inventoryItem,
            Message = "Inventory item retrieved successfully"
        });
    }

    public Task<ApiResponse<bool>> UpdateStockAsync(UpdateInventoryDto updateInventoryDto)
    {
        var inventoryItem = _inventory.FirstOrDefault(i => i.ProductId == updateInventoryDto.ProductId);
        
        if (inventoryItem == null)
        {
            return Task.FromResult(new ApiResponse<bool>
            {
                Success = false,
                Data = false,
                Message = "Inventory item not found for this product"
            });
        }

        // Create stock movement
        var stockMovement = new StockMovement
        {
            Id = _nextMovementId++,
            ProductId = updateInventoryDto.ProductId,
            Type = updateInventoryDto.MovementType,
            Quantity = updateInventoryDto.Quantity,
            Reference = updateInventoryDto.Reference,
            Notes = updateInventoryDto.Notes,
            CreatedAt = DateTime.UtcNow
        };

        // Update stock based on movement type
        switch (updateInventoryDto.MovementType)
        {
            case StockMovementType.StockIn:
            case StockMovementType.Return:
                inventoryItem.QuantityInStock += updateInventoryDto.Quantity;
                break;
            case StockMovementType.StockOut:
                if (inventoryItem.QuantityInStock < updateInventoryDto.Quantity)
                {
                    return Task.FromResult(new ApiResponse<bool>
                    {
                        Success = false,
                        Data = false,
                        Message = "Insufficient stock available"
                    });
                }
                inventoryItem.QuantityInStock -= updateInventoryDto.Quantity;
                break;
            case StockMovementType.Adjustment:
                inventoryItem.QuantityInStock = updateInventoryDto.Quantity;
                break;
        }

        inventoryItem.StockMovements.Add(stockMovement);
        inventoryItem.LastUpdated = DateTime.UtcNow;

        return Task.FromResult(new ApiResponse<bool>
        {
            Success = true,
            Data = true,
            Message = "Stock updated successfully"
        });
    }

    public Task<ApiResponse<bool>> CheckStockAvailabilityAsync(int productId, int quantity)
    {
        var inventoryItem = _inventory.FirstOrDefault(i => i.ProductId == productId);
        
        if (inventoryItem == null)
        {
            return Task.FromResult(new ApiResponse<bool>
            {
                Success = false,
                Data = false,
                Message = "Product not found in inventory"
            });
        }

        var isAvailable = inventoryItem.QuantityInStock >= quantity;

        return Task.FromResult(new ApiResponse<bool>
        {
            Success = true,
            Data = isAvailable,
            Message = isAvailable 
                ? $"Stock available. Current stock: {inventoryItem.QuantityInStock}"
                : $"Insufficient stock. Current stock: {inventoryItem.QuantityInStock}, Required: {quantity}"
        });
    }
}