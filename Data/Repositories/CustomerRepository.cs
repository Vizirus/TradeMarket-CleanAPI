using Data.Data;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;
public class CustomerRepository : AbstractRepository, ICustomerRepository
{
    public CustomerRepository(TradeMarketDbContext context)
        : base(context)
    {
    }

    public async Task AddAsync(Customer entity)
    {
        _ = await this.Context.Customers.AddAsync(entity);
    }

    public void Delete(Customer entity)
    {
        _ = this.Context.Customers.Remove(entity);
    }

    public async Task DeleteByIdAsync(int id)
    {
        Customer? customer = await this.Context.Customers.FirstOrDefaultAsync(x => x.Id == id);
        if (customer != null)
        {
            _ = this.Context.Customers.Remove(customer);
        }
    }

    public async Task<IEnumerable<Customer>> GetAllAsync()
    {
        return await this.Context.Customers.ToListAsync();
    }

    public async Task<IEnumerable<Customer>> GetAllWithDetailsAsync()
    {
        List<Customer> returnList = new ();
        var customers = await this.Context.Customers.ToListAsync();

        foreach (var i in customers)
        {
            i.Person = await this.Context.Persons.FirstOrDefaultAsync(x => x.Id == i.PersonId)
                ?? throw new ArgumentException("No such customer with this id!");
            var receipts = await this.Context.Receipts.Where(x => x.CustomerId == i.Id).ToListAsync();
            foreach (var receipt in receipts)
            {
                receipt.Customer = i;
                receipt.ReceiptDetails = await this.Context.ReceiptsDetails.Where(x => x.ReceiptId == receipt.Id).ToListAsync();
            }

            i.Receipts = receipts;
            returnList.Add(i);
        }

        return returnList;
    }

    public async Task<Customer?> GetByIdAsync(int id)
    {
        var customerList = await this.Context.Customers.ToListAsync();
        var customer = customerList.FirstOrDefault(x => x.Id == id);

        return customer;
    }

    public async Task<Customer?> GetByIdWithDetailsAsync(int id)
    {
        var i = await this.Context.Customers.FirstOrDefaultAsync(x => x.Id == id) ?? throw new ArgumentException("No such customer with this id!");
        i.Person = await this.Context.Persons.FirstOrDefaultAsync(x => x.Id == i.PersonId) ?? throw new ArgumentException("No such person with this id!");
        var receipts = await this.Context.Receipts.Where(x => x.CustomerId == i.Id).ToListAsync();
        foreach (var receipt in receipts)
        {
            receipt.Customer = i;
            receipt.ReceiptDetails = await this.Context.ReceiptsDetails.Where(x => x.ReceiptId == receipt.Id).ToListAsync();
        }

        i.Receipts = receipts;
        return i;
    }

    public void Update(Customer entity)
    {
        var existing = this.Context.Customers
         .FirstOrDefault(c => c.Id == entity.Id);

        if (existing != null)
        {
            this.Context.Entry(existing).CurrentValues.SetValues(entity);
        }
    }
}
