using Consul;
using Microsoft.Extensions.Logging;

namespace ElectronicShop.Shared.ServiceDiscovery;

public class ConsulServiceRegistry : IServiceRegistry
{
    private readonly IConsulClient _consulClient;
    private readonly ILogger<ConsulServiceRegistry> _logger;

    public ConsulServiceRegistry(IConsulClient consulClient, ILogger<ConsulServiceRegistry> logger)
    {
        _consulClient = consulClient;
        _logger = logger;
    }

    public async Task RegisterServiceAsync(ServiceRegistration registration)
    {
        var agentServiceRegistration = new AgentServiceRegistration
        {
            ID = registration.ServiceId,
            Name = registration.ServiceName,
            Address = registration.Address,
            Port = registration.Port,
            Tags = registration.Tags,
            Check = new AgentServiceCheck
            {
                HTTP = registration.HealthCheckUrl,
                Interval = registration.HealthCheckInterval,
                Timeout = TimeSpan.FromSeconds(10),
                DeregisterCriticalServiceAfter = TimeSpan.FromMinutes(5)
            }
        };

        var result = await _consulClient.Agent.ServiceRegister(agentServiceRegistration);
        
        if (result.StatusCode == System.Net.HttpStatusCode.OK)
        {
            _logger.LogInformation("Successfully registered service {ServiceName} with ID {ServiceId}", 
                registration.ServiceName, registration.ServiceId);
        }
        else
        {
            _logger.LogError("Failed to register service {ServiceName} with ID {ServiceId}. Status: {Status}", 
                registration.ServiceName, registration.ServiceId, result.StatusCode);
            throw new InvalidOperationException($"Failed to register service: {result.StatusCode}");
        }
    }

    public async Task DeregisterServiceAsync(string serviceId)
    {
        var result = await _consulClient.Agent.ServiceDeregister(serviceId);
        
        if (result.StatusCode == System.Net.HttpStatusCode.OK)
        {
            _logger.LogInformation("Successfully deregistered service with ID {ServiceId}", serviceId);
        }
        else
        {
            _logger.LogWarning("Failed to deregister service with ID {ServiceId}. Status: {Status}", 
                serviceId, result.StatusCode);
        }
    }

    public async Task<List<ServiceEntry>> DiscoverServicesAsync(string serviceName)
    {
        var result = await _consulClient.Health.Service(serviceName, string.Empty, true);
        
        if (result.StatusCode == System.Net.HttpStatusCode.OK)
        {
            _logger.LogDebug("Discovered {Count} instances of service {ServiceName}", 
                result.Response.Length, serviceName);
            
            return result.Response.ToList();
        }
        
        _logger.LogWarning("Failed to discover services for {ServiceName}. Status: {Status}", 
            serviceName, result.StatusCode);
        
        return new List<ServiceEntry>();
    }

    public async Task<ServiceEntry?> DiscoverServiceAsync(string serviceName)
    {
        var services = await DiscoverServicesAsync(serviceName);
        
        if (services.Count == 0)
        {
            _logger.LogWarning("No healthy instances found for service {ServiceName}", serviceName);
            return null;
        }

        // Simple round-robin selection (could be enhanced with load balancing strategies)
        var random = new Random();
        var selectedService = services[random.Next(services.Count)];
        
        _logger.LogDebug("Selected service instance {ServiceId} for {ServiceName}", 
            selectedService.Service.ID, serviceName);
        
        return selectedService;
    }
}