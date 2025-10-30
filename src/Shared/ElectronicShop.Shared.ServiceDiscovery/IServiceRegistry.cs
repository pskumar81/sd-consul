using Consul;

namespace ElectronicShop.Shared.ServiceDiscovery;

public class ServiceRegistration
{
    public string ServiceId { get; set; } = string.Empty;
    public string ServiceName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public int Port { get; set; }
    public string[] Tags { get; set; } = Array.Empty<string>();
    public TimeSpan HealthCheckInterval { get; set; } = TimeSpan.FromSeconds(30);
    public string HealthCheckUrl { get; set; } = string.Empty;
}

public class ServiceDiscoveryConfig
{
    public string ConsulAddress { get; set; } = "http://localhost:8500";
    public string DataCenter { get; set; } = "dc1";
}

public interface IServiceRegistry
{
    Task RegisterServiceAsync(ServiceRegistration registration);
    Task DeregisterServiceAsync(string serviceId);
    Task<List<ServiceEntry>> DiscoverServicesAsync(string serviceName);
    Task<ServiceEntry?> DiscoverServiceAsync(string serviceName);
}