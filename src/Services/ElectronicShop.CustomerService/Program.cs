using ElectronicShop.CustomerService.Services;
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
        Title = "Electronic Shop - Customer Service", 
        Version = "v1",
        Description = "Customer management service for electronic shop"
    });
});

// Register application services
builder.Services.AddScoped<ICustomerService, ElectronicShop.CustomerService.Services.CustomerService>();

// Add Consul service discovery
builder.Services.AddConsulServiceDiscovery(builder.Configuration);

// Configure service registration
var serviceRegistration = new ServiceRegistration
{
    ServiceId = $"customer-service-{Environment.MachineName}-{Environment.ProcessId}",
    ServiceName = "customer-service",
    Address = "localhost",
    Port = 5002,
    Tags = new[] { "api", "customer", "v1" },
    HealthCheckUrl = "http://localhost:5002/api/health",
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
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Customer Service V1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
