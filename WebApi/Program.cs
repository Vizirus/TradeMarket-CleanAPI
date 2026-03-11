using System.Runtime.InteropServices;
using Business;
using Business.Interfaces;
using Business.Services;
using Business.Validation;
using Data.Data;
using Data.Interfaces;
using Data.Repositories;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using WebApi.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddDbContext<TradeMarketDbContext>(opts =>
{
    if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX) || RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
    {
        var connectionString = builder.Configuration.GetConnectionString("Market");
        _ = opts.UseSqlite(connectionString ?? "Data Source=trade_market.db");
    }
    else
    {
        // Use SQL Server on Windows
        var connectionString = builder.Configuration.GetConnectionString("MarketSqlServer");
        _ = opts.UseSqlServer(connectionString ?? "Server=(localdb)\\mssqllocaldb;Database=TradeMarket;Trusted_Connection=True;");
    }

});
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IReceiptRepository, ReceiptRepository>();
builder.Services.AddScoped<IReceiptService, ReceiptService>();
builder.Services.AddScoped<IReceiptRepository, ReceiptRepository>();
builder.Services.AddScoped<IReceiptService, ReceiptService>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IStatisticService, StatisticService>();
builder.Services.AddScoped<IPersonRepository, PersonRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<AutomapperProfile>());
builder.Services.AddScoped<DataSeeder>();

// TODO: Register business layer services

// TODO: Configure development environment features

// TODO: Configure middleware pipeline

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    _ = app.UseDeveloperExceptionPage();
}

//using (var context = app.Services.CreateScope())
//{
//    var dataSeader = context.ServiceProvider.GetService<DataSeeder>();
//    dataSeader!.Seed();
//}

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
        context.Response.ContentType = "application/json";

        if (exception is ArgumentException || exception is MarketException)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsync("{\"error\": \"" + exception.Message + "\"}");
        }
        else
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsync(exception!.Message);
        }
    });
});

app.MapControllers();

await app.RunAsync();

// 🚨 CRITICAL: DO NOT DELETE THIS CLASS!
// This partial class is required for integration testing with WebApplicationFactory
// It allows test projects to access the Program class and configure the test host
// Removing this class will break all integration tests in WebApi.Tests project
// This is a .NET 6+ pattern for enabling testing of top-level programs
public partial class Program { }
