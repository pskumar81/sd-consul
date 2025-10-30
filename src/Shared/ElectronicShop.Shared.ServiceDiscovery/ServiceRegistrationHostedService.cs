using Consul;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ElectronicShop.Shared.ServiceDiscovery;

public class ServiceRegistrationHostedService : IHostedService
{
    private readonly IServiceRegistry _serviceRegistry;
    private readonly ServiceRegistration _serviceRegistration;
    private readonly ILogger<ServiceRegistrationHostedService> _logger;

    public ServiceRegistrationHostedService(
        IServiceRegistry serviceRegistry,
        ServiceRegistration serviceRegistration,
        ILogger<ServiceRegistrationHostedService> logger)
    {
        _serviceRegistry = serviceRegistry;
        _serviceRegistration = serviceRegistration;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _serviceRegistry.RegisterServiceAsync(_serviceRegistration);
            _logger.LogInformation("Service registration completed for {ServiceName}", _serviceRegistration.ServiceName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to register service {ServiceName}", _serviceRegistration.ServiceName);
            throw;
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _serviceRegistry.DeregisterServiceAsync(_serviceRegistration.ServiceId);
            _logger.LogInformation("Service deregistration completed for {ServiceName}", _serviceRegistration.ServiceName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to deregister service {ServiceName}", _serviceRegistration.ServiceName);
        }
    }
}