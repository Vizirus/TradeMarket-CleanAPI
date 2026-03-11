namespace Business.Models;

public class ReceiptDetailModel
{
    public ReceiptDetailModel()
    {
    }

    public ReceiptDetailModel(int id, int receiptId, int productId, int quantity, decimal unitPrice, decimal discountUnitPrice)
    {
        this.Id = id;
        this.ReceiptId = receiptId;
        this.ProductId = productId;
        this.Quantity = quantity;
        this.UnitPrice = unitPrice;
        this.DiscountUnitPrice = discountUnitPrice;
    }

    public int Id { get; set; }

    public int ReceiptId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal DiscountUnitPrice { get; set; }

    public override string ToString()
    {
        return $"Id - {this.Id}\nReceiptId - {this.ReceiptId}\nProductId - {this.ProductId}\nQuantity - {this.Quantity}\nUnitPrice - {this.UnitPrice}\nDiscountUnitPrice - {this.DiscountUnitPrice}";
    }
}
