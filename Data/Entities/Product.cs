using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class Product : BaseEntity
{
    [Column]
    [ForeignKey("ProductCategory")]
    public int ProductCategoryId { get; set; }

    [Column]
    public string? ProductName { get; set; }

    [Column]
    public decimal Price { get; set; }

    public ProductCategory? Category { get; set; }

    public IList<ReceiptDetail>? ReceiptDetails { get; set; }
}
