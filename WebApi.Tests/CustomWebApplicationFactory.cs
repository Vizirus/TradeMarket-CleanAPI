using Data.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace WebApi.Tests;

/// <summary>
/// Custom factory for WebApi integration tests with full isolation and in-memory database.
/// </summary>
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
		builder.ConfigureServices(services =>
		{
			// Generate a unique database name for every test run
			var databaseName = $"TestDb_{Guid.NewGuid()}";

			// Remove the existing DbContext registration
			var descriptor = services.SingleOrDefault(
				d => d.ServiceType == typeof(DbContextOptions<TradeMarketDbContext>));
			if (descriptor != null)
			{
				services.Remove(descriptor);
			}

			// Register in-memory DbContext with unique database name for the test instance
			_ = services.AddDbContext<TradeMarketDbContext>(options =>
			{
				options.UseInMemoryDatabase(databaseName);
				options.EnableSensitiveDataLogging();
			});

			// Log the database name for debugging
			var provider = services.BuildServiceProvider();
			var logger = provider.GetService<ILogger<CustomWebApplicationFactory>>();
			logger?.LogInformation($"Using in-memory database: {databaseName}");
		});

        builder.UseEnvironment("Testing");
    }

    protected override void ConfigureClient(HttpClient client)
    {
        base.ConfigureClient(client);

        // Initialize database once after the application is built
        using var scope = this.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<TradeMarketDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<CustomWebApplicationFactory>>();

        try
        {
            db.Database.EnsureCreated();
            UnitTestDataHelper.SeedData(db);
            logger.LogInformation("Test database initialized successfully");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error initializing test database");
            throw;
        }
    }
}
