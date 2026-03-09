using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class ProductCategory : BaseEntity
{
    public ProductCategory()
    {
        this.CategoryName = string.Empty;
    }

    public ProductCategory(int id, string categoryName)
        : base(id)
    {
        this.CategoryName = categoryName;
    }

    [Column]
    public string CategoryName { get; set; }

    public IList<Product>? Products { get; set; }
}
