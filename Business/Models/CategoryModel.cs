namespace Business.Models;

public class CategoryModel
{
    public CategoryModel(int id, string name)
    {
        this.Id = id;
        this.Name = name;
    }

    public CategoryModel()
    {
    }

    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public IList<int> ProductIds { get; set; } = new List<int>();

    public override string ToString()
    {
        return $"Id - {this.Id}\nName - {this.Name}";
    }
}
