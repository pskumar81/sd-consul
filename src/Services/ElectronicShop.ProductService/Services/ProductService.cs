using ElectronicShop.Shared.Models;

namespace ElectronicShop.ProductService.Services;

public interface IProductService
{
    Task<ApiResponse<IEnumerable<Product>>> GetAllProductsAsync();
    Task<ApiResponse<Product?>> GetProductByIdAsync(int id);
    Task<ApiResponse<Product>> CreateProductAsync(CreateProductDto createProductDto);
    Task<ApiResponse<Product?>> UpdateProductAsync(int id, UpdateProductDto updateProductDto);
    Task<ApiResponse<bool>> DeleteProductAsync(int id);
    Task<ApiResponse<IEnumerable<Product>>> GetProductsByCategoryAsync(string category);
    Task<ApiResponse<IEnumerable<Product>>> SearchProductsAsync(string searchTerm);
}

public class ProductService : IProductService
{
    private static readonly List<Product> _products = new()
    {
        new Product
        {
            Id = 1,
            Name = "iPhone 15 Pro",
            Description = "Latest iPhone with advanced camera system",
            Price = 999.99m,
            Category = "Smartphones",
            Brand = "Apple",
            Model = "iPhone 15 Pro",
            Specifications = new List<string> { "6.1-inch display", "A17 Pro chip", "48MP camera", "128GB storage" },
            ImageUrl = "https://example.com/iphone15pro.jpg",
            CreatedAt = DateTime.UtcNow.AddDays(-30),
            UpdatedAt = DateTime.UtcNow.AddDays(-5),
            IsActive = true
        },
        new Product
        {
            Id = 2,
            Name = "Samsung Galaxy S24",
            Description = "Premium Android smartphone with AI features",
            Price = 849.99m,
            Category = "Smartphones",
            Brand = "Samsung",
            Model = "Galaxy S24",
            Specifications = new List<string> { "6.2-inch display", "Snapdragon 8 Gen 3", "50MP camera", "256GB storage" },
            ImageUrl = "https://example.com/galaxys24.jpg",
            CreatedAt = DateTime.UtcNow.AddDays(-25),
            UpdatedAt = DateTime.UtcNow.AddDays(-3),
            IsActive = true
        },
        new Product
        {
            Id = 3,
            Name = "MacBook Pro 14\"",
            Description = "Professional laptop with M3 chip",
            Price = 1999.99m,
            Category = "Laptops",
            Brand = "Apple",
            Model = "MacBook Pro 14\"",
            Specifications = new List<string> { "14-inch Liquid Retina XDR", "M3 chip", "16GB RAM", "512GB SSD" },
            ImageUrl = "https://example.com/macbookpro14.jpg",
            CreatedAt = DateTime.UtcNow.AddDays(-20),
            UpdatedAt = DateTime.UtcNow.AddDays(-2),
            IsActive = true
        },
        new Product
        {
            Id = 4,
            Name = "Sony WH-1000XM5",
            Description = "Wireless noise-canceling headphones",
            Price = 399.99m,
            Category = "Audio",
            Brand = "Sony",
            Model = "WH-1000XM5",
            Specifications = new List<string> { "30-hour battery", "Advanced ANC", "Bluetooth 5.2", "Quick charge" },
            ImageUrl = "https://example.com/sonywh1000xm5.jpg",
            CreatedAt = DateTime.UtcNow.AddDays(-15),
            UpdatedAt = DateTime.UtcNow.AddDays(-1),
            IsActive = true
        }
    };

    private static int _nextId = 5;

    public Task<ApiResponse<IEnumerable<Product>>> GetAllProductsAsync()
    {
        var activeProducts = _products.Where(p => p.IsActive).ToList();
        return Task.FromResult(new ApiResponse<IEnumerable<Product>>
        {
            Success = true,
            Data = activeProducts,
            Message = $"Retrieved {activeProducts.Count} products"
        });
    }

    public Task<ApiResponse<Product?>> GetProductByIdAsync(int id)
    {
        var product = _products.FirstOrDefault(p => p.Id == id && p.IsActive);
        
        if (product == null)
        {
            return Task.FromResult(new ApiResponse<Product?>
            {
                Success = false,
                Data = null,
                Message = "Product not found"
            });
        }

        return Task.FromResult(new ApiResponse<Product?>
        {
            Success = true,
            Data = product,
            Message = "Product retrieved successfully"
        });
    }

    public Task<ApiResponse<Product>> CreateProductAsync(CreateProductDto createProductDto)
    {
        var product = new Product
        {
            Id = _nextId++,
            Name = createProductDto.Name,
            Description = createProductDto.Description,
            Price = createProductDto.Price,
            Category = createProductDto.Category,
            Brand = createProductDto.Brand,
            Model = createProductDto.Model,
            Specifications = createProductDto.Specifications,
            ImageUrl = createProductDto.ImageUrl,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            IsActive = true
        };

        _products.Add(product);

        return Task.FromResult(new ApiResponse<Product>
        {
            Success = true,
            Data = product,
            Message = "Product created successfully"
        });
    }

    public Task<ApiResponse<Product?>> UpdateProductAsync(int id, UpdateProductDto updateProductDto)
    {
        var product = _products.FirstOrDefault(p => p.Id == id && p.IsActive);
        
        if (product == null)
        {
            return Task.FromResult(new ApiResponse<Product?>
            {
                Success = false,
                Data = null,
                Message = "Product not found"
            });
        }

        if (!string.IsNullOrEmpty(updateProductDto.Name))
            product.Name = updateProductDto.Name;
        
        if (!string.IsNullOrEmpty(updateProductDto.Description))
            product.Description = updateProductDto.Description;
        
        if (updateProductDto.Price.HasValue)
            product.Price = updateProductDto.Price.Value;
        
        if (!string.IsNullOrEmpty(updateProductDto.Category))
            product.Category = updateProductDto.Category;
        
        if (!string.IsNullOrEmpty(updateProductDto.Brand))
            product.Brand = updateProductDto.Brand;
        
        if (!string.IsNullOrEmpty(updateProductDto.Model))
            product.Model = updateProductDto.Model;
        
        if (updateProductDto.Specifications != null)
            product.Specifications = updateProductDto.Specifications;
        
        if (!string.IsNullOrEmpty(updateProductDto.ImageUrl))
            product.ImageUrl = updateProductDto.ImageUrl;
        
        if (updateProductDto.IsActive.HasValue)
            product.IsActive = updateProductDto.IsActive.Value;

        product.UpdatedAt = DateTime.UtcNow;

        return Task.FromResult(new ApiResponse<Product?>
        {
            Success = true,
            Data = product,
            Message = "Product updated successfully"
        });
    }

    public Task<ApiResponse<bool>> DeleteProductAsync(int id)
    {
        var product = _products.FirstOrDefault(p => p.Id == id && p.IsActive);
        
        if (product == null)
        {
            return Task.FromResult(new ApiResponse<bool>
            {
                Success = false,
                Data = false,
                Message = "Product not found"
            });
        }

        product.IsActive = false;
        product.UpdatedAt = DateTime.UtcNow;

        return Task.FromResult(new ApiResponse<bool>
        {
            Success = true,
            Data = true,
            Message = "Product deleted successfully"
        });
    }

    public Task<ApiResponse<IEnumerable<Product>>> GetProductsByCategoryAsync(string category)
    {
        var products = _products
            .Where(p => p.IsActive && p.Category.Equals(category, StringComparison.OrdinalIgnoreCase))
            .ToList();

        return Task.FromResult(new ApiResponse<IEnumerable<Product>>
        {
            Success = true,
            Data = products,
            Message = $"Retrieved {products.Count} products in category '{category}'"
        });
    }

    public Task<ApiResponse<IEnumerable<Product>>> SearchProductsAsync(string searchTerm)
    {
        var products = _products
            .Where(p => p.IsActive && 
                       (p.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                        p.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                        p.Brand.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                        p.Category.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)))
            .ToList();

        return Task.FromResult(new ApiResponse<IEnumerable<Product>>
        {
            Success = true,
            Data = products,
            Message = $"Found {products.Count} products matching '{searchTerm}'"
        });
    }
}