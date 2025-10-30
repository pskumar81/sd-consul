namespace ElectronicShop.Shared.Models;

public class InventoryItem
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public Product? Product { get; set; }
    public int QuantityInStock { get; set; }
    public int ReorderLevel { get; set; }
    public int MaxStockLevel { get; set; }
    public DateTime LastUpdated { get; set; }
    public List<StockMovement> StockMovements { get; set; } = new();
}

public class StockMovement
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public StockMovementType Type { get; set; }
    public int Quantity { get; set; }
    public string Reference { get; set; } = string.Empty; // Order ID, Supplier reference, etc.
    public string Notes { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public enum StockMovementType
{
    StockIn = 1,
    StockOut = 2,
    Adjustment = 3,
    Return = 4
}

public class UpdateInventoryDto
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public StockMovementType MovementType { get; set; }
    public string Reference { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
}

public class CreateInventoryItemDto
{
    public int ProductId { get; set; }
    public int QuantityInStock { get; set; }
    public int ReorderLevel { get; set; }
    public int MaxStockLevel { get; set; }
}