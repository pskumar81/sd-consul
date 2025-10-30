using ElectronicShop.ProductService.Services;
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
        Title = "Electronic Shop - Product Service", 
        Version = "v1",
        Description = "Product management service for electronic shop"
    });
});

// Register application services
builder.Services.AddScoped<IProductService, ElectronicShop.ProductService.Services.ProductService>();

// Add Consul service discovery
builder.Services.AddConsulServiceDiscovery(builder.Configuration);

// Configure service registration
var serviceRegistration = new ServiceRegistration
{
    ServiceId = $"product-service-{Environment.MachineName}-{Environment.ProcessId}",
    ServiceName = "product-service",
    Address = "localhost",
    Port = 5001,
    Tags = new[] { "api", "product", "v1" },
    HealthCheckUrl = "http://localhost:5001/api/health",
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
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Product Service V1");
        c.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
