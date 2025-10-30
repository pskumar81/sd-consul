namespace ElectronicShop.Shared.Models;

public class Order
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public Customer? Customer { get; set; }
    public List<OrderItem> Items { get; set; } = new();
    public decimal TotalAmount { get; set; }
    public OrderStatus Status { get; set; }
    public Address ShippingAddress { get; set; } = new();
    public DateTime OrderDate { get; set; }
    public DateTime? ShippedDate { get; set; }
    public DateTime? DeliveredDate { get; set; }
    public string Notes { get; set; } = string.Empty;
}

public class OrderItem
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public Product? Product { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
}

public enum OrderStatus
{
    Pending = 0,
    Confirmed = 1,
    Processing = 2,
    Shipped = 3,
    Delivered = 4,
    Cancelled = 5,
    Returned = 6
}

public class CreateOrderDto
{
    public int CustomerId { get; set; }
    public List<CreateOrderItemDto> Items { get; set; } = new();
    public Address ShippingAddress { get; set; } = new();
    public string Notes { get; set; } = string.Empty;
}

public class CreateOrderItemDto
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}

public class UpdateOrderStatusDto
{
    public OrderStatus Status { get; set; }
    public string Notes { get; set; } = string.Empty;
}