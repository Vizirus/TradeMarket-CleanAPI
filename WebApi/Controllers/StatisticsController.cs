using Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StatisticsController : Controller
{
    private readonly IStatisticService statisticsService;

    public StatisticsController(IStatisticService statisticsService)
    {
        this.statisticsService = statisticsService;
    }

    [HttpGet("most-popular-products")]
    public async Task<IActionResult> GetMostPopularProducts([FromQuery] int count)
    {
        var products = await this.statisticsService.GetMostPopularProductsAsync(count);
        return this.Ok(products);
    }

    [HttpGet("customers-most-popular-products")]
    public async Task<IActionResult> GetCustomersMostPopularProducts([FromQuery] int count, [FromQuery] int customerId)
    {
        var products = await this.statisticsService.GetCustomersMostPopularProductsAsync(count, customerId);
        return this.Ok(products);
    }

    [HttpGet("most-valuable-customers")]
    public async Task<IActionResult> GetMostValuableCustomers([FromQuery] int count, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        var customers = await this.statisticsService.GetMostValuableCustomersAsync(count, startDate, endDate);
        return this.Ok(customers);
    }

    [HttpGet("income-of-category")]
    public async Task<IActionResult> GetIncomeOfCategory([FromQuery] int categoryId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        var income = await this.statisticsService.GetIncomeOfCategoryInPeriod(categoryId, startDate, endDate);
        return this.Ok(income);
    }
}
