using ElectronicShop.Shared.Models;
using ElectronicShop.ProductService.Services;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicShop.ProductService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(IProductService productService, ILogger<ProductsController> logger)
    {
        _productService = productService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<Product>>>> GetAllProducts()
    {
        _logger.LogInformation("Getting all products");
        var result = await _productService.GetAllProductsAsync();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<Product?>>> GetProduct(int id)
    {
        _logger.LogInformation("Getting product with ID: {ProductId}", id);
        var result = await _productService.GetProductByIdAsync(id);
        
        if (!result.Success)
        {
            return NotFound(result);
        }
        
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<Product>>> CreateProduct(CreateProductDto createProductDto)
    {
        _logger.LogInformation("Creating new product: {ProductName}", createProductDto.Name);
        var result = await _productService.CreateProductAsync(createProductDto);
        
        return CreatedAtAction(
            nameof(GetProduct), 
            new { id = result.Data!.Id }, 
            result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<Product?>>> UpdateProduct(int id, UpdateProductDto updateProductDto)
    {
        _logger.LogInformation("Updating product with ID: {ProductId}", id);
        var result = await _productService.UpdateProductAsync(id, updateProductDto);
        
        if (!result.Success)
        {
            return NotFound(result);
        }
        
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteProduct(int id)
    {
        _logger.LogInformation("Deleting product with ID: {ProductId}", id);
        var result = await _productService.DeleteProductAsync(id);
        
        if (!result.Success)
        {
            return NotFound(result);
        }
        
        return Ok(result);
    }

    [HttpGet("category/{category}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<Product>>>> GetProductsByCategory(string category)
    {
        _logger.LogInformation("Getting products in category: {Category}", category);
        var result = await _productService.GetProductsByCategoryAsync(category);
        return Ok(result);
    }

    [HttpGet("search")]
    public async Task<ActionResult<ApiResponse<IEnumerable<Product>>>> SearchProducts([FromQuery] string q)
    {
        if (string.IsNullOrWhiteSpace(q))
        {
            return BadRequest(new ApiResponse<IEnumerable<Product>>
            {
                Success = false,
                Message = "Search query cannot be empty"
            });
        }

        _logger.LogInformation("Searching products with query: {SearchQuery}", q);
        var result = await _productService.SearchProductsAsync(q);
        return Ok(result);
    }
}