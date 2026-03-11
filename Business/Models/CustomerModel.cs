namespace Business.Models;

public class CustomerModel
{
    public CustomerModel(int id, string name, string surname, DateTime birthDate, decimal discountValue, IList<int> receiptsIds)
    {
        this.Id = id;
        this.Name = name;
        this.Surname = surname;
        this.BirthDate = birthDate;
        this.DiscountValue = discountValue;
        this.ReceiptsIds = receiptsIds;
    }

    public CustomerModel()
    {
    }

    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Surname { get; set; } = string.Empty;

    public DateTime BirthDate { get; set; }

    public decimal DiscountValue { get; set; }

    public IList<int> ReceiptsIds { get; set; } = new List<int>();

    public override string ToString()
    {
        return $"Id - {this.Id}\nName - {this.Name}\nSurname - {this.Surname}\nBirthDate - {this.BirthDate:yyyy-MM-dd}\nDiscountValue - {this.DiscountValue}\nReceiptsIds - [{string.Join(", ", this.ReceiptsIds)}]";
    }
}
