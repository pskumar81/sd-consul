using ElectronicShop.Shared.Models;
using ElectronicShop.Shared.ServiceDiscovery;
using Consul;
using Newtonsoft.Json;
using System.Text;

namespace ElectronicShop.ConsoleClient.Services;

public class ApiClientService
{
    private readonly HttpClient _httpClient;
    private readonly IServiceRegistry _serviceRegistry;

    public ApiClientService(HttpClient httpClient, IServiceRegistry serviceRegistry)
    {
        _httpClient = httpClient;
        _serviceRegistry = serviceRegistry;
    }

    public async Task<ApiResponse<T>?> GetAsync<T>(string serviceName, string endpoint)
    {
        var service = await _serviceRegistry.DiscoverServiceAsync(serviceName);
        if (service == null)
        {
            Console.WriteLine($"❌ Service '{serviceName}' not found");
            return null;
        }

        var url = $"http://{service.Service.Address}:{service.Service.Port}{endpoint}";
        
        try
        {
            var response = await _httpClient.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiResponse<T>>(content);
            }
            else
            {
                Console.WriteLine($"❌ Error calling {serviceName}: {response.StatusCode} - {content}");
                return null;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Exception calling {serviceName}: {ex.Message}");
            return null;
        }
    }

    public async Task<ApiResponse<T>?> PostAsync<T>(string serviceName, string endpoint, object data)
    {
        var service = await _serviceRegistry.DiscoverServiceAsync(serviceName);
        if (service == null)
        {
            Console.WriteLine($"❌ Service '{serviceName}' not found");
            return null;
        }

        var url = $"http://{service.Service.Address}:{service.Service.Port}{endpoint}";
        
        try
        {
            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PostAsync(url, content);
            var responseContent = await response.Content.ReadAsStringAsync();
            
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiResponse<T>>(responseContent);
            }
            else
            {
                Console.WriteLine($"❌ Error calling {serviceName}: {response.StatusCode} - {responseContent}");
                return null;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Exception calling {serviceName}: {ex.Message}");
            return null;
        }
    }

    public async Task DisplayAvailableServicesAsync()
    {
        Console.WriteLine("\n🔍 Discovering available services...");
        
        var services = new[] { "product-service", "customer-service", "inventory-service" };
        
        foreach (var serviceName in services)
        {
            var service = await _serviceRegistry.DiscoverServiceAsync(serviceName);
            if (service != null)
            {
                Console.WriteLine($"✅ {serviceName} - {service.Service.Address}:{service.Service.Port}");
            }
            else
            {
                Console.WriteLine($"❌ {serviceName} - Not available");
            }
        }
    }
}