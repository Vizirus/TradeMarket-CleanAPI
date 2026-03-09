using Data.Entities;

namespace Data.Data;
internal static class DataPopulator
{
    public static IEnumerable<Customer> PopulateCustomers()
    {
        return new List<Customer>
        {
            new Customer(1, 1, 10),
            new Customer(2, 2, 5),
        };
    }

    public static IEnumerable<Person> PopulatePersons()
    {
        return new List<Person>
        {
            new Person(1, "John", "Doe", new DateTime(new DateOnly(1999, 1, 1), new TimeOnly(0, 0, 0), DateTimeKind.Utc)),
            new Person(2, "Jane", "Smith", new DateTime(new DateOnly(1990, 5, 15), new TimeOnly(0, 0, 0), DateTimeKind.Utc)),
        };
    }

    public static IEnumerable<Product> PopulateProducts()
    {
        return new List<Product>
        {
            new Product(1, 1, "Apple", 1.50m),
            new Product(2, 1, "Banana", 1.00m),
            new Product(3, 2, "Shampoo", 5.99m),
        };
    }

    public static IEnumerable<ProductCategory> PopulateProductCategory()
    {
        return new List<ProductCategory>
        {
            new ProductCategory(1, "Fruits"),
            new ProductCategory(2, "Cosmetics"),
        };
    }

    public static IEnumerable<Receipt> PopulateReceipts()
    {
        return new List<Receipt>
        {
            new Receipt(1, 1, new DateTime(new DateOnly(2024, 2, 1), new TimeOnly(0, 0, 0), DateTimeKind.Utc), true),
            new Receipt(2, 2, new DateTime(new DateOnly(2024, 2, 4), new TimeOnly(0, 0, 0), DateTimeKind.Utc), false),
        };
    }

    public static IEnumerable<ReceiptDetail> PopulateReceiptDetails()
    {
        return new List<ReceiptDetail>
        {
            new ReceiptDetail(1, 1, 1, 1.50m, 1.35m, 2),
            new ReceiptDetail(2, 1, 2, 1.00m, 0.95m, 3),
            new ReceiptDetail(3, 2, 3, 5.99m, 5.50m, 1),
        };
    }
}
