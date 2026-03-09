using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class Person : BaseEntity
{
    [Column("Name")]
    public string? Name { get; set; }

    [Column("Surname")]
    public string? Surname { get; set; }

    [Column("BirthDate")]
    public DateTime? BirthDate { get; set; }
}
