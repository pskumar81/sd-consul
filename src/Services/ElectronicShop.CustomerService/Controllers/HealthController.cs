using ElectronicShop.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicShop.CustomerService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public ActionResult<ServiceHealth> GetHealth()
    {
        return Ok(new ServiceHealth
        {
            ServiceName = "Customer Service",
            Status = "Healthy",
            Version = "1.0.0",
            Timestamp = DateTime.UtcNow,
            AdditionalInfo = new Dictionary<string, object>
            {
                { "Environment", Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development" },
                { "MachineName", Environment.MachineName }
            }
        });
    }
}