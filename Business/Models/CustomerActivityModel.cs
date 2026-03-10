namespace Business.Models;

public class CustomerActivityModel
{
    public CustomerActivityModel(int customerId, string customerName, decimal receiptSum)
    {
        this.CustomerId = customerId;
        this.CustomerName = customerName;
        this.ReceiptSum = receiptSum;
    }

    public CustomerActivityModel()
    {
    }

    public int CustomerId { get; set; }

    public string CustomerName { get; set; } = null!;

    public decimal ReceiptSum { get; set; }

    public override string ToString()
    {
        return $"CustomerId - {this.CustomerId}\nCustomerName - {this.CustomerName}\nReceiptSum - {this.ReceiptSum}";
    }
}
