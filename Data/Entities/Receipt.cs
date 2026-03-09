using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class Receipt : BaseEntity
{
    public Receipt()
    {
    }

    public Receipt(int id, int customerId, DateTime OperationDate, bool isCheckedOut)
        : base(id)
    {
        this.OperationDate = OperationDate;
        this.CustomerId = customerId;
        this.IsCheckedOut = isCheckedOut;
    }

    [Column]
    [ForeignKey("Customer")]
    public int CustomerId { get; set; }

    [Column]
    public DateTime OperationDate { get; set; }

    [Column]
    public bool IsCheckedOut { get; set; }

    public Customer Customer { get; set; }

    public IList<ReceiptDetail> ReceiptDetails { get; set; }
}
