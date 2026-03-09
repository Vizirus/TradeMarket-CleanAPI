using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class Receipt : BaseEntity
{
    [Column]
    [ForeignKey("Customer")]
    public int CustomerId { get; set; }

    [Column]
    public DateTime OperationDate { get; set; }

    [Column]
    public bool IsCheckedOut { get; set; }

    public Customer? Customer { get; set; }

    public IList<ReceiptDetail>? ReceiptDetails { get; set; }
}
