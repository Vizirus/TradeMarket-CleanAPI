namespace Business.Models;

public class ProductModel
{
    public ProductModel()
    {
    }

    public ProductModel(int id, string productName, decimal price, int productCategoryId, string categoryName, IList<int> receiptDetailIds)
    {
        this.Id = id;
        this.ProductName = productName;
        this.Price = price;
        this.ProductCategoryId = productCategoryId;
        this.CategoryName = categoryName;
        this.ReceiptDetailIds = receiptDetailIds;
    }

    public int Id { get; set; }

    public string ProductName { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public int ProductCategoryId { get; set; }

    public string CategoryName { get; set; } = string.Empty;

    public IList<int> ReceiptDetailIds { get; set; } = new List<int>();

    public override string ToString()
    {
        return $"Id - {this.Id}\nProductName - {this.ProductName}\nPrice - {this.Price}\nProductCategoryId - {this.ProductCategoryId}\nCategoryName - {this.CategoryName}\nReceiptDetailIds - [{string.Join(", ", this.ReceiptDetailIds)}]";
    }
}
