using ElectronicShop.Shared.Models;

namespace ElectronicShop.CustomerService.Services;

public interface ICustomerService
{
    Task<ApiResponse<IEnumerable<Customer>>> GetAllCustomersAsync();
    Task<ApiResponse<Customer?>> GetCustomerByIdAsync(int id);
    Task<ApiResponse<Customer>> CreateCustomerAsync(CreateCustomerDto createCustomerDto);
    Task<ApiResponse<Customer?>> UpdateCustomerAsync(int id, UpdateCustomerDto updateCustomerDto);
    Task<ApiResponse<bool>> DeleteCustomerAsync(int id);
    Task<ApiResponse<Customer?>> GetCustomerByEmailAsync(string email);
}

public class CustomerService : ICustomerService
{
    private static readonly List<Customer> _customers = new()
    {
        new Customer
        {
            Id = 1,
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@email.com",
            Phone = "+1-555-0101",
            Address = new Address
            {
                Street = "123 Main St",
                City = "New York",
                State = "NY",
                Country = "USA",
                PostalCode = "10001"
            },
            CreatedAt = DateTime.UtcNow.AddDays(-60),
            UpdatedAt = DateTime.UtcNow.AddDays(-10),
            IsActive = true
        },
        new Customer
        {
            Id = 2,
            FirstName = "Jane",
            LastName = "Smith",
            Email = "jane.smith@email.com",
            Phone = "+1-555-0102",
            Address = new Address
            {
                Street = "456 Oak Ave",
                City = "Los Angeles",
                State = "CA",
                Country = "USA",
                PostalCode = "90210"
            },
            CreatedAt = DateTime.UtcNow.AddDays(-45),
            UpdatedAt = DateTime.UtcNow.AddDays(-5),
            IsActive = true
        }
    };

    private static int _nextId = 3;

    public Task<ApiResponse<IEnumerable<Customer>>> GetAllCustomersAsync()
    {
        var activeCustomers = _customers.Where(c => c.IsActive).ToList();
        return Task.FromResult(new ApiResponse<IEnumerable<Customer>>
        {
            Success = true,
            Data = activeCustomers,
            Message = $"Retrieved {activeCustomers.Count} customers"
        });
    }

    public Task<ApiResponse<Customer?>> GetCustomerByIdAsync(int id)
    {
        var customer = _customers.FirstOrDefault(c => c.Id == id && c.IsActive);
        
        if (customer == null)
        {
            return Task.FromResult(new ApiResponse<Customer?>
            {
                Success = false,
                Data = null,
                Message = "Customer not found"
            });
        }

        return Task.FromResult(new ApiResponse<Customer?>
        {
            Success = true,
            Data = customer,
            Message = "Customer retrieved successfully"
        });
    }

    public Task<ApiResponse<Customer>> CreateCustomerAsync(CreateCustomerDto createCustomerDto)
    {
        // Check if email already exists
        if (_customers.Any(c => c.Email.Equals(createCustomerDto.Email, StringComparison.OrdinalIgnoreCase) && c.IsActive))
        {
            return Task.FromResult(new ApiResponse<Customer>
            {
                Success = false,
                Data = null!,
                Message = "Customer with this email already exists"
            });
        }

        var customer = new Customer
        {
            Id = _nextId++,
            FirstName = createCustomerDto.FirstName,
            LastName = createCustomerDto.LastName,
            Email = createCustomerDto.Email,
            Phone = createCustomerDto.Phone,
            Address = createCustomerDto.Address,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            IsActive = true
        };

        _customers.Add(customer);

        return Task.FromResult(new ApiResponse<Customer>
        {
            Success = true,
            Data = customer,
            Message = "Customer created successfully"
        });
    }

    public Task<ApiResponse<Customer?>> UpdateCustomerAsync(int id, UpdateCustomerDto updateCustomerDto)
    {
        var customer = _customers.FirstOrDefault(c => c.Id == id && c.IsActive);
        
        if (customer == null)
        {
            return Task.FromResult(new ApiResponse<Customer?>
            {
                Success = false,
                Data = null,
                Message = "Customer not found"
            });
        }

        // Check if email already exists for another customer
        if (!string.IsNullOrEmpty(updateCustomerDto.Email) && 
            _customers.Any(c => c.Id != id && c.Email.Equals(updateCustomerDto.Email, StringComparison.OrdinalIgnoreCase) && c.IsActive))
        {
            return Task.FromResult(new ApiResponse<Customer?>
            {
                Success = false,
                Data = null,
                Message = "Another customer with this email already exists"
            });
        }

        if (!string.IsNullOrEmpty(updateCustomerDto.FirstName))
            customer.FirstName = updateCustomerDto.FirstName;
        
        if (!string.IsNullOrEmpty(updateCustomerDto.LastName))
            customer.LastName = updateCustomerDto.LastName;
        
        if (!string.IsNullOrEmpty(updateCustomerDto.Email))
            customer.Email = updateCustomerDto.Email;
        
        if (!string.IsNullOrEmpty(updateCustomerDto.Phone))
            customer.Phone = updateCustomerDto.Phone;
        
        if (updateCustomerDto.Address != null)
            customer.Address = updateCustomerDto.Address;
        
        if (updateCustomerDto.IsActive.HasValue)
            customer.IsActive = updateCustomerDto.IsActive.Value;

        customer.UpdatedAt = DateTime.UtcNow;

        return Task.FromResult(new ApiResponse<Customer?>
        {
            Success = true,
            Data = customer,
            Message = "Customer updated successfully"
        });
    }

    public Task<ApiResponse<bool>> DeleteCustomerAsync(int id)
    {
        var customer = _customers.FirstOrDefault(c => c.Id == id && c.IsActive);
        
        if (customer == null)
        {
            return Task.FromResult(new ApiResponse<bool>
            {
                Success = false,
                Data = false,
                Message = "Customer not found"
            });
        }

        customer.IsActive = false;
        customer.UpdatedAt = DateTime.UtcNow;

        return Task.FromResult(new ApiResponse<bool>
        {
            Success = true,
            Data = true,
            Message = "Customer deleted successfully"
        });
    }

    public Task<ApiResponse<Customer?>> GetCustomerByEmailAsync(string email)
    {
        var customer = _customers.FirstOrDefault(c => c.Email.Equals(email, StringComparison.OrdinalIgnoreCase) && c.IsActive);
        
        if (customer == null)
        {
            return Task.FromResult(new ApiResponse<Customer?>
            {
                Success = false,
                Data = null,
                Message = "Customer not found"
            });
        }

        return Task.FromResult(new ApiResponse<Customer?>
        {
            Success = true,
            Data = customer,
            Message = "Customer retrieved successfully"
        });
    }
}