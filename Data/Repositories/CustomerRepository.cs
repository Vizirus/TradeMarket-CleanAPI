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
        ArgumentNullException.ThrowIfNull(nameof(entity));
        bool validation = ValidateEntity(entity);
        if (validation)
        {
            _ = await this.Context.Customers.AddAsync(entity);
            _ = await this.Context.SaveChangesAsync();
        }
    }

    public void Delete(Customer entity)
    {
        ArgumentNullException.ThrowIfNull(nameof(entity));
        bool validation = ValidateEntity(entity);
        if (validation)
        {
            _ = this.Context.Customers.Remove(entity);
            _ = this.Context.SaveChanges();
        }
    }

    public async Task DeleteByIdAsync(int id)
    {
        if (id >= 0)
        {
            Customer? productCategory = await this.Context.Customers.FirstOrDefaultAsync(x => x.Id == id);
            if (productCategory != null)
            {
                _ = this.Context.Customers.Remove(productCategory);
                _ = await this.Context.SaveChangesAsync();
                return;
            }

            throw new ArgumentException("The id of customer is inncorect! Could find any customers!");
        }

        throw new ArgumentException("The id of customer is inncorect! Id was negative!");
    }

    public async Task<IEnumerable<Customer>> GetAllAsync()
    {
        return await this.Context.Customers.ToListAsync();
    }

    public async Task<IEnumerable<Customer>> GetAllWithDetailsAsync()
    {
        List<Customer> returnList = new ();
        foreach (var i in this.Context.Customers)
        {
            i.Person = await this.Context.Persons.FirstOrDefaultAsync(x => x.Id == i.PersonId) ?? throw new ArgumentException("No such customer with this id!");
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
        if (id >= 0)
        {
            var category = await this.Context.Customers.FirstOrDefaultAsync(x => x.Id == id);
            if (category is null)
            {
                throw new ArgumentException("The id of customer is inncorect! Could find any customers!");
            }

            return category;
        }

        throw new ArgumentException("The id of customer is inncorect! Id was negative!");
    }

    public async Task<Customer?> GetByIdWithDetailsAsync(int id)
    {
        if (id >= 0)
        {
            var i = await this.Context.Customers.FirstOrDefaultAsync(x => x.Id == id) ?? throw new ArgumentException("No such customer with this id!");
            i.Person = await this.Context.Persons.FirstOrDefaultAsync(x => x.Id == i.PersonId) ?? throw new ArgumentException("No such customer with this id!");
            var receipts = await this.Context.Receipts.Where(x => x.CustomerId == i.Id).ToListAsync();
            foreach (var receipt in receipts)
            {
                receipt.Customer = i;
                receipt.ReceiptDetails = await this.Context.ReceiptsDetails.Where(x => x.ReceiptId == receipt.Id).ToListAsync();
            }

            i.Receipts = receipts;
            return i;
        }

        throw new ArgumentException("Could not customer with this id! Id was negative!");
    }

    public void Update(Customer entity)
    {
        ArgumentNullException.ThrowIfNull(nameof(entity));
        bool validation = ValidateEntity(entity);
        if (validation)
        {
            _ = this.Context.Customers.Update(entity);
            _ = this.Context.SaveChanges();
        }
    }
}
