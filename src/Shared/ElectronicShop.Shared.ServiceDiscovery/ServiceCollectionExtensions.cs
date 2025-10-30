using Consul;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace ElectronicShop.Shared.ServiceDiscovery;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddConsulServiceDiscovery(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        var config = configuration.GetSection("ServiceDiscovery").Get<ServiceDiscoveryConfig>() 
                    ?? new ServiceDiscoveryConfig();

        services.AddSingleton<IConsulClient>(provider =>
        {
            return new ConsulClient(consulConfig =>
            {
                consulConfig.Address = new Uri(config.ConsulAddress);
                consulConfig.Datacenter = config.DataCenter;
            });
        });

        services.AddSingleton<IServiceRegistry, ConsulServiceRegistry>();
        
        return services;
    }

    public static IServiceCollection AddServiceRegistration(
        this IServiceCollection services,
        ServiceRegistration registration)
    {
        services.AddSingleton(registration);
        services.AddHostedService<ServiceRegistrationHostedService>();
        
        return services;
    }
}