using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Data;

public class TradeMarketDbContext : DbContext
{
    public DbSet<Customer> Customers { get; set; }

    public DbSet<Person> Persons { get; set; }

    public DbSet<Product> Products { get; set; }

    public DbSet<ProductCategory> ProductCategories { get; set; }

    public DbSet<Receipt> Receipts { get; set; }

    public DbSet<ReceiptDetail> ReceiptsDetails { get; set; }

    public TradeMarketDbContext(DbContextOptions options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);
        _ = modelBuilder.Entity<Customer>().HasData(DataPopulator.PopulateCustomers());
        _ = modelBuilder.Entity<Person>().HasData(DataPopulator.PopulatePersons());
        _ = modelBuilder.Entity<Product>().HasData(DataPopulator.PopulateProducts());
        _ = modelBuilder.Entity<ProductCategory>().HasData(DataPopulator.PopulateProductCategory());
        _ = modelBuilder.Entity<Receipt>().HasData(DataPopulator.PopulateReceipts());
        _ = modelBuilder.Entity<ReceiptDetail>().HasData(DataPopulator.PopulateReceiptDetails());
    }
}
