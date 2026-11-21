using System;
using System.Net;
using Business.Interfaces;
using Business.Models;
using Business.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : Controller
{
    private readonly ICustomerService service;

    public CustomersController(ICustomerService service)
    {
        this.service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var resultModel = await this.service.GetAllAsync();
        if (!resultModel.Any())
        {
            return this.NotFound();
        }
        else
        {
            return this.Ok(resultModel);
        }

    }

    [HttpGet()]
    [Route("/api/customers/{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await this.service.GetByIdAsync(id);
        if (result != null)
        {
            return this.Ok(result);
        }
        return this.NotFound();
    }

    [HttpGet]
    [Route("/api/customers/by-product/{id:int}")]
    public async Task<IActionResult> GetByProductID(int id)
    {
        var result = await this.service.GetCustomersByProductIdAsync(id);
        if (result != null)
        {
            return this.Ok(result);
        }
        return this.Ok(new List<CustomerModel>());
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CustomerModel customerModel)
    {
        if (!this.ModelState.IsValid)
        {
            return this.BadRequest(this.ModelState);
        }
        var createdCustomer = await this.service.AddAsync(customerModel);

        return this.CreatedAtAction(
            nameof(GetById),
            new { id = createdCustomer.Id },
            createdCustomer
        );
    }

    [HttpPut]
    [Route("/api/customers/{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] CustomerModel customerModel)
    {
        var result = await this.service.GetByIdAsync(id);
        if (result != null && customerModel.Id == result.Id)
        {
            await this.service.UpdateAsync(customerModel);
        }
        else
        {
            return this.BadRequest();
        }

        return this.Created();
    }

    [HttpDelete]
    [Route("/api/customers/{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await this.service.DeleteAsync(id);
        return this.NoContent();
    }
}
