using ElectronicShop.Shared.ServiceDiscovery;
using ElectronicShop.ConsoleClient.Services;
using ElectronicShop.Shared.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;

namespace ElectronicShop.ConsoleClient;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("🏪 Electronic Shop - Order Management Console Client");
        Console.WriteLine("==================================================");

        var builder = Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                // Add HTTP client
                services.AddHttpClient<ApiClientService>();
                
                // Add Consul service discovery
                services.AddConsulServiceDiscovery(context.Configuration);
                
                // Add our API client service
                services.AddScoped<ApiClientService>();
            });

        var host = builder.Build();
        
        using var scope = host.Services.CreateScope();
        var apiClient = scope.ServiceProvider.GetRequiredService<ApiClientService>();

        // Wait a moment for services to register
        await Task.Delay(2000);

        await RunInteractiveMenuAsync(apiClient);
    }

    static async Task RunInteractiveMenuAsync(ApiClientService apiClient)
    {
        while (true)
        {
            Console.WriteLine("\n📋 Main Menu:");
            Console.WriteLine("1. Show Available Services");
            Console.WriteLine("2. Product Management");
            Console.WriteLine("3. Customer Management");
            Console.WriteLine("4. Inventory Management");
            Console.WriteLine("5. Create Sample Order");
            Console.WriteLine("0. Exit");
            Console.Write("\nEnter your choice: ");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    await apiClient.DisplayAvailableServicesAsync();
                    break;
                case "2":
                    await ShowProductMenu(apiClient);
                    break;
                case "3":
                    await ShowCustomerMenu(apiClient);
                    break;
                case "4":
                    await ShowInventoryMenu(apiClient);
                    break;
                case "5":
                    await CreateSampleOrder(apiClient);
                    break;
                case "0":
                    Console.WriteLine("👋 Goodbye!");
                    return;
                default:
                    Console.WriteLine("❌ Invalid choice. Please try again.");
                    break;
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }

    static async Task ShowProductMenu(ApiClientService apiClient)
    {
        Console.WriteLine("\n📱 Product Management:");
        Console.WriteLine("1. List all products");
        Console.WriteLine("2. Search products");
        Console.WriteLine("3. Get products by category");
        Console.Write("\nEnter your choice: ");

        var choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                var products = await apiClient.GetAsync<IEnumerable<Product>>("product-service", "/api/products");
                if (products?.Success == true && products.Data != null)
                {
                    Console.WriteLine("\n📦 Products:");
                    foreach (var product in products.Data)
                    {
                        Console.WriteLine($"  {product.Id}. {product.Name} - ${product.Price:F2} ({product.Category})");
                    }
                }
                break;
            case "2":
                Console.Write("Enter search term: ");
                var searchTerm = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    var searchResults = await apiClient.GetAsync<IEnumerable<Product>>("product-service", $"/api/products/search?q={searchTerm}");
                    if (searchResults?.Success == true && searchResults.Data != null)
                    {
                        Console.WriteLine($"\n🔍 Search Results for '{searchTerm}':");
                        foreach (var product in searchResults.Data)
                        {
                            Console.WriteLine($"  {product.Id}. {product.Name} - ${product.Price:F2}");
                        }
                    }
                }
                break;
            case "3":
                Console.Write("Enter category: ");
                var category = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(category))
                {
                    var categoryProducts = await apiClient.GetAsync<IEnumerable<Product>>("product-service", $"/api/products/category/{category}");
                    if (categoryProducts?.Success == true && categoryProducts.Data != null)
                    {
                        Console.WriteLine($"\n📂 Products in category '{category}':");
                        foreach (var product in categoryProducts.Data)
                        {
                            Console.WriteLine($"  {product.Id}. {product.Name} - ${product.Price:F2}");
                        }
                    }
                }
                break;
        }
    }

    static async Task ShowCustomerMenu(ApiClientService apiClient)
    {
        Console.WriteLine("\n👥 Customer Management:");
        Console.WriteLine("1. List all customers");
        Console.WriteLine("2. Find customer by email");
        Console.Write("\nEnter your choice: ");

        var choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                var customers = await apiClient.GetAsync<IEnumerable<Customer>>("customer-service", "/api/customers");
                if (customers?.Success == true && customers.Data != null)
                {
                    Console.WriteLine("\n👥 Customers:");
                    foreach (var customer in customers.Data)
                    {
                        Console.WriteLine($"  {customer.Id}. {customer.FirstName} {customer.LastName} - {customer.Email}");
                    }
                }
                break;
            case "2":
                Console.Write("Enter email: ");
                var email = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(email))
                {
                    var customer = await apiClient.GetAsync<Customer>("customer-service", $"/api/customers/email/{email}");
                    if (customer?.Success == true && customer.Data != null)
                    {
                        var c = customer.Data;
                        Console.WriteLine($"\n👤 Customer Found:");
                        Console.WriteLine($"  Name: {c.FirstName} {c.LastName}");
                        Console.WriteLine($"  Email: {c.Email}");
                        Console.WriteLine($"  Phone: {c.Phone}");
                        Console.WriteLine($"  Address: {c.Address.Street}, {c.Address.City}, {c.Address.State}");
                    }
                }
                break;
        }
    }

    static async Task ShowInventoryMenu(ApiClientService apiClient)
    {
        Console.WriteLine("\n📦 Inventory Management:");
        Console.WriteLine("1. Check stock for product");
        Console.Write("\nEnter your choice: ");

        var choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                Console.Write("Enter product ID: ");
                if (int.TryParse(Console.ReadLine(), out int productId))
                {
                    var inventory = await apiClient.GetAsync<InventoryItem>("inventory-service", $"/api/inventory/product/{productId}");
                    if (inventory?.Success == true && inventory.Data != null)
                    {
                        var item = inventory.Data;
                        Console.WriteLine($"\n📦 Inventory for Product ID {productId}:");
                        Console.WriteLine($"  Quantity in Stock: {item.QuantityInStock}");
                        Console.WriteLine($"  Reorder Level: {item.ReorderLevel}");
                        Console.WriteLine($"  Max Stock Level: {item.MaxStockLevel}");
                        Console.WriteLine($"  Last Updated: {item.LastUpdated:yyyy-MM-dd HH:mm:ss}");
                        
                        if (item.QuantityInStock <= item.ReorderLevel)
                        {
                            Console.WriteLine("  ⚠️  Low stock warning!");
                        }
                    }
                }
                break;
        }
    }

    static async Task CreateSampleOrder(ApiClientService apiClient)
    {
        Console.WriteLine("\n🛒 Creating Sample Order...");
        
        // First, get a customer
        var customers = await apiClient.GetAsync<IEnumerable<Customer>>("customer-service", "/api/customers");
        if (customers?.Success != true || customers.Data?.Any() != true)
        {
            Console.WriteLine("❌ No customers available");
            return;
        }

        var customer = customers.Data.First();
        Console.WriteLine($"👤 Customer: {customer.FirstName} {customer.LastName}");

        // Get some products
        var products = await apiClient.GetAsync<IEnumerable<Product>>("product-service", "/api/products");
        if (products?.Success != true || products.Data?.Any() != true)
        {
            Console.WriteLine("❌ No products available");
            return;
        }

        var productList = products.Data.Take(2).ToList();
        
        Console.WriteLine("\n📱 Selected Products for Order:");
        decimal totalAmount = 0;
        foreach (var product in productList)
        {
            var quantity = 1;
            var itemTotal = product.Price * quantity;
            totalAmount += itemTotal;
            
            Console.WriteLine($"  - {product.Name}: {quantity} x ${product.Price:F2} = ${itemTotal:F2}");
            
            // Check inventory
            var stockCheck = await apiClient.GetAsync<bool>("inventory-service", $"/api/inventory/check-availability?productId={product.Id}&quantity={quantity}");
            if (stockCheck?.Success == true && stockCheck.Data)
            {
                Console.WriteLine($"    ✅ Stock available");
            }
            else
            {
                Console.WriteLine($"    ❌ Insufficient stock");
                return;
            }
        }

        Console.WriteLine($"\n💰 Total Order Amount: ${totalAmount:F2}");
        Console.WriteLine("✅ Sample order would be valid!");
        Console.WriteLine("(Note: Order service implementation would handle the actual order creation and stock reduction)");
    }
}
