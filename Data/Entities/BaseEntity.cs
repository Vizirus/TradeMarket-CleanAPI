using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public abstract class BaseEntity
{
    protected BaseEntity(int id)
    {
        this.Id = id;
    }

    protected BaseEntity()
    {
    }

    [Key]
    [Column]
    public int Id { get; set; }
}
