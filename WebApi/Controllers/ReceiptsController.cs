using Business.Interfaces;
using Business.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReceiptsController : Controller
{
    private readonly IReceiptService receiptService;

    public ReceiptsController(IReceiptService receiptService)
    {
        this.receiptService = receiptService;
    }

    // CRUD
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var receipts = await this.receiptService.GetAllAsync();
        if (receipts == null || !receipts.Any())
        {
            return this.NotFound();
        }
        return this.Ok(receipts);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var receipt = await this.receiptService.GetByIdAsync(id);
        if (receipt == null)
        {
            return this.NotFound();
        }
        return this.Ok(receipt);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ReceiptModel receipt)
    {
        if (!this.ModelState.IsValid)
        {
            return this.BadRequest(this.ModelState);
        }
        var created = await this.receiptService.AddAsync(receipt);
        return this.CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] ReceiptModel receipt)
    {
        if (!this.ModelState.IsValid || id != receipt.Id)
        {
            return this.BadRequest(this.ModelState);
        }
        var exists = await this.receiptService.GetByIdAsync(id);
        if (exists == null)
        {
            return this.NotFound();
        }
        await this.receiptService.UpdateAsync(receipt);
        return this.NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await this.receiptService.DeleteAsync(id);
        return this.NoContent();
    }

    [HttpPost("{receiptId:int}/products/{productId:int}")]
    public async Task<IActionResult> AddProduct(int receiptId, int productId, [FromQuery] int quantity)
    {
        await this.receiptService.AddProductAsync(productId, receiptId, quantity);
        var result = this.receiptService.GetByIdAsync(productId);
        if (result is null)
        {
            return this.BadRequest();
        }
        return this.Ok();
    }

    // Remove product from receipt
    [HttpDelete("{receiptId:int}/products/{productId:int}")]
    public async Task<IActionResult> RemoveProduct(int receiptId, int productId, [FromQuery] int quantity)
    {
        await this.receiptService.RemoveProductAsync(productId, receiptId, quantity);
        return this.Ok();
    }

    // Get receipt details
    [HttpGet("{receiptId:int}/details")]
    public async Task<IActionResult> GetReceiptDetails(int receiptId)
    {
        var details = await this.receiptService.GetReceiptDetailsAsync(receiptId);
        if (details == null)
        {
            return this.BadRequest();
        }
        return this.Ok(details);
    }

    [HttpGet("{receiptId:int}/total")]
    public async Task<IActionResult> ToPay(int receiptId)
    {
        var total = await this.receiptService.ToPayAsync(receiptId);
        return this.Ok(total);
    }

    // Checkout
    [HttpPost("{receiptId:int}/checkout")]
    public async Task<IActionResult> CheckOut(int receiptId)
    {
        await this.receiptService.CheckOutAsync(receiptId);
        return this.Ok();
    }

    // Get receipts by period
    [HttpGet("period")]
    public async Task<IActionResult> GetReceiptsByPeriod([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        var receipts = await this.receiptService.GetReceiptsByPeriodAsync(startDate, endDate);
        return this.Ok(receipts);
    }
}
