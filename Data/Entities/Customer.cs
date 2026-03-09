using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class Customer : BaseEntity
{
    public Customer()
    {
        this.PersonId = 0;
        this.DiscountValue = 0;
    }

    public Customer(int id, int personId, int discountValue)
        : base(id)
    {
        this.PersonId = personId;
        this.DiscountValue = discountValue;
    }

    [Column]
    [ForeignKey("Person")]
    public int PersonId { get; set; }

    [Column]
    public int DiscountValue { get; set; }

    public Person? Person { get; set; }

    public IList<Receipt>? Receipts { get; set; }
}
