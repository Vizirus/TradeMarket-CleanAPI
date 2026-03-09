using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class Product : BaseEntity
{
    public Product()
    {
        this.ProductCategoryId = 0;
        this.ProductName = string.Empty;
        this.Price = 0;
    }

    public Product(int id, int productCategoryId, string productName, decimal price)
        : base(id)
    {
        this.ProductCategoryId = productCategoryId;
        this.ProductName = productName;
        this.Price = price;
    }

    [Column]
    [ForeignKey("ProductCategory")]
    public int ProductCategoryId { get; set; }

    [Column]
    public string ProductName { get; set; }

    [Column]
    public decimal Price { get; set; }

    public ProductCategory? Category { get; set; }

    public IList<ReceiptDetail>? ReceiptDetails { get; set; }
}
