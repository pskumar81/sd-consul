using ElectronicShop.Shared.Models;
using ElectronicShop.CustomerService.Services;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicShop.CustomerService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly ICustomerService _customerService;
    private readonly ILogger<CustomersController> _logger;

    public CustomersController(ICustomerService customerService, ILogger<CustomersController> logger)
    {
        _customerService = customerService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<Customer>>>> GetAllCustomers()
    {
        _logger.LogInformation("Getting all customers");
        var result = await _customerService.GetAllCustomersAsync();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<Customer?>>> GetCustomer(int id)
    {
        _logger.LogInformation("Getting customer with ID: {CustomerId}", id);
        var result = await _customerService.GetCustomerByIdAsync(id);
        
        if (!result.Success)
        {
            return NotFound(result);
        }
        
        return Ok(result);
    }

    [HttpGet("email/{email}")]
    public async Task<ActionResult<ApiResponse<Customer?>>> GetCustomerByEmail(string email)
    {
        _logger.LogInformation("Getting customer with email: {Email}", email);
        var result = await _customerService.GetCustomerByEmailAsync(email);
        
        if (!result.Success)
        {
            return NotFound(result);
        }
        
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<Customer>>> CreateCustomer(CreateCustomerDto createCustomerDto)
    {
        _logger.LogInformation("Creating new customer: {Email}", createCustomerDto.Email);
        var result = await _customerService.CreateCustomerAsync(createCustomerDto);
        
        if (!result.Success)
        {
            return BadRequest(result);
        }
        
        return CreatedAtAction(
            nameof(GetCustomer), 
            new { id = result.Data!.Id }, 
            result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<Customer?>>> UpdateCustomer(int id, UpdateCustomerDto updateCustomerDto)
    {
        _logger.LogInformation("Updating customer with ID: {CustomerId}", id);
        var result = await _customerService.UpdateCustomerAsync(id, updateCustomerDto);
        
        if (!result.Success)
        {
            return NotFound(result);
        }
        
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteCustomer(int id)
    {
        _logger.LogInformation("Deleting customer with ID: {CustomerId}", id);
        var result = await _customerService.DeleteCustomerAsync(id);
        
        if (!result.Success)
        {
            return NotFound(result);
        }
        
        return Ok(result);
    }
}