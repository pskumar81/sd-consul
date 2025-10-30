using ElectronicShop.InventoryService.Services;
using ElectronicShop.Shared.ServiceDiscovery;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "Electronic Shop - Inventory Service", 
        Version = "v1",
        Description = "Inventory management service for electronic shop"
    });
});

// Register application services
builder.Services.AddScoped<IInventoryService, ElectronicShop.InventoryService.Services.InventoryService>();

// Add Consul service discovery
builder.Services.AddConsulServiceDiscovery(builder.Configuration);

// Configure service registration
var serviceRegistration = new ServiceRegistration
{
    ServiceId = $"inventory-service-{Environment.MachineName}-{Environment.ProcessId}",
    ServiceName = "inventory-service",
    Address = "localhost",
    Port = 5003,
    Tags = new[] { "api", "inventory", "v1" },
    HealthCheckUrl = "http://localhost:5003/api/health",
    HealthCheckInterval = TimeSpan.FromSeconds(30)
};

builder.Services.AddServiceRegistration(serviceRegistration);

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => 
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Inventory Service V1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
