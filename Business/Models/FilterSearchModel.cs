namespace Business.Models;

public class FilterSearchModel
{
    public FilterSearchModel()
    {
    }

    public FilterSearchModel(string? query, int? categoryId, decimal? minPrice, decimal? maxPrice, bool? inStock)
    {
        this.Query = query;
        this.CategoryId = categoryId;
        this.MinPrice = minPrice;
        this.MaxPrice = maxPrice;
        this.InStock = inStock;
    }

    public string? Query { get; set; } = string.Empty;

    public int? CategoryId { get; set; }

    public decimal? MinPrice { get; set; }

    public decimal? MaxPrice { get; set; }

    public bool? InStock { get; set; }

    public override string ToString()
    {
        return $"Query - {this.Query}\nCategoryId - {this.CategoryId}\nMinPrice - {this.MinPrice}\nMaxPrice - {this.MaxPrice}\nInStock - {this.InStock}";
    }
}
