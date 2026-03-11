using Business.Interfaces;
using Business.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : Controller
{
    private readonly IProductService productService;

    public ProductsController(IProductService productService)
    {
        this.productService = productService;
    }

    [HttpPost("filter")]
    public async Task<IActionResult> GetByFilter([FromBody] FilterSearchModel filter)
    {
        var products = await this.productService.GetByFilterAsync(filter);
        return this.Ok(products);
    }

    [HttpGet("categories")]
    public async Task<IActionResult> GetAllProductCategories()
    {
        var categories = await this.productService.GetAllProductCategoriesAsync();
        if (categories == null || !categories.Any())
        {
            return this.NotFound();
        }
        return this.Ok(categories);
    }

    [HttpPost("categories")]
    public async Task<IActionResult> AddCategory([FromBody] CategoryModel category)
    {
        if (!this.ModelState.IsValid || string.IsNullOrWhiteSpace(category.Name))
        {
            return this.BadRequest(this.ModelState);
        }

        await this.productService.AddCategoryAsync(category);
        return this.CreatedAtAction(nameof(GetAllProductCategories), new { id = category.Id }, category);
    }

    [HttpPut("categories/{id:int}")]
    public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryModel category)
    {
        var allCategories = await this.productService.GetAllProductCategoriesAsync();
        var result = allCategories.FirstOrDefault(x => x.Id == id);
        if (result != null && result.Id == category.Id)
        {
            await this.productService.UpdateCategoryAsync(category);
        }
        else
        {
            return this.BadRequest();
        }

        return this.Created();
    }

    [HttpDelete("categories/{id:int}")]
    public async Task<IActionResult> RemoveCategory(int id)
    {
        var allCategories = await this.productService.GetAllProductCategoriesAsync();
        var exist = allCategories.FirstOrDefault(x => x.Id == id);
        if (exist == null)
        {
            return this.NoContent();
        }

        await this.productService.RemoveCategoryAsync(id);
        return this.NoContent();
    }

    [HttpGet]
    public async Task<IActionResult> GetAllProducts()
    {
        var products = await this.productService.GetAllAsync();
        if (products == null || !products.Any())
        {
            return this.NotFound();
        }

        return this.Ok(products);
    }

    [HttpGet("/api/products/{id:int}")]
    public async Task<IActionResult> GetProductById(int id)
    {
        var product = await this.productService.GetByIdAsync(id);
        if (product == null)
        {
            return this.NotFound();
        }

        return this.Ok(product);
    }

    [HttpPost()]
    public async Task<IActionResult> CreateProduct([FromBody] ProductModel product)
    {
        if (!this.ModelState.IsValid || string.IsNullOrWhiteSpace(product.ProductName) || product.Price < 0)
        {
            return this.BadRequest(this.ModelState);
        }

        var created = await this.productService.AddAsync(product);
        return this.CreatedAtAction(nameof(GetProductById), new { id = created.Id }, created);
    }

    [HttpPut("/api/products/{id:int}")]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductModel product)
    {
        if (!this.ModelState.IsValid || id != product.Id)
        {
            return this.BadRequest(this.ModelState);
        }

        var exists = await this.productService.GetByIdAsync(id);
        if (exists == null)
        {
            return this.NotFound();
        }

        await this.productService.UpdateAsync(product);
        return this.NoContent();
    }

    [HttpDelete("/api/products/{id:int}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        await this.productService.DeleteAsync(id);
        return this.NoContent();
    }
}

