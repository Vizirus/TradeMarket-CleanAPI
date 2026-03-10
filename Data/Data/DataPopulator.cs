using Data.Entities;

namespace Data.Data;
internal static class DataPopulator
{
    public static IEnumerable<Customer> PopulateCustomers()
    {
        return new List<Customer>
        {
            new Customer { Id = 1, PersonId = 1, DiscountValue = 2 },
            new Customer { Id = 2, PersonId = 2, DiscountValue = 5 },
        };
    }

    public static IEnumerable<Person> PopulatePersons()
    {
        return new List<Person>
        {
            new Person { Id = 1, Name = "John", Surname = "Doe", BirthDate = new DateTime(1999, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Person { Id = 2, Name = "Jane", Surname = "Smith", BirthDate = new DateTime(1990, 5, 15, 0, 0, 0, DateTimeKind.Utc) },
        };
    }

    public static IEnumerable<Product> PopulateProducts()
    {
        return new List<Product>
        {
            new Product { Id = 1, ProductCategoryId = 1, ProductName = "Apple", Price = 1.50m },
            new Product { Id = 2, ProductCategoryId = 1, ProductName = "Banana", Price = 1.00m },
            new Product { Id = 3, ProductCategoryId = 2, ProductName = "Shampoo", Price = 5.99m },
        };
    }

    public static IEnumerable<ProductCategory> PopulateProductCategory()
    {
        return new List<ProductCategory>
        {
            new ProductCategory { Id = 1, CategoryName = "Fruits" },
            new ProductCategory { Id = 2, CategoryName = "Cosmetics" },
        };
    }

    public static IEnumerable<Receipt> PopulateReceipts()
    {
        return new List<Receipt>
        {
            new Receipt { Id = 1, CustomerId = 1, OperationDate = new DateTime(2024, 2, 1, 0, 0, 0, DateTimeKind.Utc), IsCheckedOut = true },
            new Receipt { Id = 2, CustomerId = 2, OperationDate = new DateTime(2024, 2, 4, 0, 0, 0, DateTimeKind.Utc), IsCheckedOut = false },
        };
    }

    public static IEnumerable<ReceiptDetail> PopulateReceiptDetails()
    {
        return new List<ReceiptDetail>
        {
            new ReceiptDetail { Id = 1, ReceiptId = 1, ProductId = 1, UnitPrice = 1.50m, DiscountUnitPrice = 1.35m, Quantity = 2 },
            new ReceiptDetail { Id = 2, ReceiptId = 1, ProductId = 2, UnitPrice = 1.00m, DiscountUnitPrice = 0.95m, Quantity = 3 },
            new ReceiptDetail { Id = 3, ReceiptId = 2, ProductId = 3, UnitPrice = 5.99m, DiscountUnitPrice = 5.50m, Quantity = 1 },
        };
    }
}
