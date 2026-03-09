using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class ProductCategory : BaseEntity
{
    [Column]
    public string? CategoryName { get; set; }

    public IList<Product>? Products { get; set; }
}
