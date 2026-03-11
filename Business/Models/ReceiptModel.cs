namespace Business.Models;

public class ReceiptModel
{
    public ReceiptModel()
    {
    }

    public ReceiptModel(int id, int customerId, bool isCheckedOut, DateTime operationDate, IList<int> receiptDetailsIds)
    {
        this.Id = id;
        this.CustomerId = customerId;
        this.IsCheckedOut = isCheckedOut;
        this.OperationDate = operationDate;
        this.ReceiptDetailsIds = receiptDetailsIds;
    }

    public int Id { get; set; }

    public int CustomerId { get; set; }

    public bool IsCheckedOut { get; set; }

    public DateTime OperationDate { get; set; }

    public IList<int> ReceiptDetailsIds { get; set; } = new List<int>();

    public override string ToString()
    {
        return $"Id - {this.Id}\nCustomerId - {this.CustomerId}\nIsCheckedOut - {this.IsCheckedOut}\nOperationDate - {this.OperationDate:yyyy-MM-dd}\nReceiptDetailsIds - [{string.Join(", ", this.ReceiptDetailsIds)}]";
    }
}
