namespace ElectronicShop.Shared.Models;

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public T? Data { get; set; }
    public string Message { get; set; } = string.Empty;
    public List<string> Errors { get; set; } = new();
}

public class PagedResult<T>
{
    public List<T> Items { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;
}

public class ServiceHealth
{
    public string ServiceName { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public Dictionary<string, object> AdditionalInfo { get; set; } = new();
}