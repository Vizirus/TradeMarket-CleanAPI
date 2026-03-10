using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Data.Entities;

public class ReceiptDetail : BaseEntity
{
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

    public Receipt? Receipt { get; set; }

    public Product? Product { get; set; }
}
