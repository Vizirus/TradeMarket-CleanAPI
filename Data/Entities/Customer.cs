using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;
public class Customer : BaseEntity
{
    [Column]
    [ForeignKey("Person")]
    public int PersonId { get; set; }

    [Column]
    public int DiscountValue { get; set; }

    public Person? Person { get; set; }

    public IList<Receipt>? Receipts { get; set; }
}
