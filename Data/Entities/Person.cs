using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class Person : BaseEntity
{
    public Person()
    {
        this.Name = string.Empty;
        this.Surname = string.Empty;
        this.BirthDate = DateTime.MaxValue;
    }

    public Person(int id, string firstName, string lastName, DateTime birthDate)
        : base(id)
    {
        this.Name = firstName;
        this.Surname = lastName;
        this.BirthDate = birthDate;
    }

    [Column("Name")]
    public string Name { get; set; }

    [Column("Surname")]
    public string Surname { get; set; }

    [Column("BirthDate")]
    public DateTime BirthDate { get; set; }
}
