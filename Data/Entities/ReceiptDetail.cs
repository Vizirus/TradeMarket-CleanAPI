using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Data.Entities;

public class ReceiptDetail : BaseEntity
{
    public ReceiptDetail()
    {
    }

    public ReceiptDetail(int id, int receiptId, int productId, decimal unitPrice, decimal discountUnitPrice, int quality)
        : base(id)
    {
        this.Quantity = quality;
        this.ReceiptId = receiptId;
        this.ProductId = productId;
        this.UnitPrice = unitPrice;
        this.DiscountUnitPrice = discountUnitPrice;
    }

    [Column]
    [ForeignKey("Receipts")]
    public int ReceiptId { get; set; }

    [Column]
    [ForeignKey("Products")]
    public int ProductId { get; set; }

    [Column]
    public decimal UnitPrice { get; set; }

    [Column]
    public decimal DiscountUnitPrice { get; set; }

    [Column]
    public int Quantity { get; set; }

    public Receipt Receipt { get; set; }

    public Product Product { get; set; }
}
