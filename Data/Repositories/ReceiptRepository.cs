using Data.Data;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class ReceiptRepository : AbstractRepository, IReceiptRepository
{
    public ReceiptRepository(TradeMarketDbContext context)
        : base(context)
    {
    }

    public async Task AddAsync(Receipt entity)
    {
        ArgumentNullException.ThrowIfNull(nameof(entity));
        bool validation = ValidateEntity(entity);
        if (validation)
        {
            _ = await this.context.Receipts.AddAsync(entity);
            _ = await this.context.SaveChangesAsync();
        }
    }

    public void Delete(Receipt entity)
    {
        ArgumentNullException.ThrowIfNull(nameof(entity));
        bool validation = ValidateEntity(entity);
        if (validation)
        {
            _ = this.context.Receipts.Remove(entity);
            _ = this.context.SaveChanges();
        }
    }

    public async Task DeleteByIdAsync(int id)
    {
        if (id >= 0)
        {
            var productCategory = await this.context.Receipts.FirstOrDefaultAsync(x => x.Id == id);
            if (productCategory != null)
            {
                _ = this.context.Receipts.Remove(productCategory);
                _ = await this.context.SaveChangesAsync();
                return;
            }

            throw new ArgumentException("The id of receipt is inncorect! Could find any receipts!");
        }

        throw new ArgumentException("The id of receipt is inncorect! Id was negative!");
    }

    public async Task<IEnumerable<Receipt>> GetAllAsync()
    {
        return await this.context.Receipts.ToListAsync();
    }

    public async Task<IEnumerable<Receipt>> GetAllWithDetailsAsync()
    {
        List<Receipt> returnList = new();
        foreach (var receipt in this.context.Receipts)
        {
            receipt.Customer = await this.context.Customers.FirstOrDefaultAsync(x => x.Id == receipt.CustomerId)
                ?? throw new ArgumentException("No such customer with this id!");

            var details = await this.context.ReceiptsDetails.Where(x => x.ReceiptId == receipt.Id).ToListAsync();
            foreach (var detail in details)
            {
                detail.Receipt = receipt;
                detail.Product = await this.context.Products.FirstOrDefaultAsync(x => x.Id == detail.ProductId)
                    ?? throw new ArgumentException("No such product with this id!");
                detail.Product.Category = await this.context.ProductCategories.FirstOrDefaultAsync(x => x.Id == detail.Product.ProductCategoryId) ?? throw new ArgumentException("No such category!");
            }

            receipt.ReceiptDetails = details;
            returnList.Add(receipt);
        }

        return returnList;
    }

    public async Task<Receipt?> GetByIdAsync(int id)
    {
        if (id >= 0)
        {
            var category = await this.context.Receipts.FirstOrDefaultAsync(x => x.Id == id);
            if (category is null)
            {
                throw new ArgumentException("The id of receipt is inncorect! Could find any receipts!");
            }

            return category;
        }

        throw new ArgumentException("The id of receipt is inncorect! Id was negative!");
    }

    public async Task<Receipt?> GetByIdWithDetailsAsync(int id)
    {
        if (id < 0)
        {
            throw new ArgumentException("The id of receipt is incorrect! Id was negative!");
        }

        var receipt = await this.context.Receipts.FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new ArgumentException("No such receipt with this id!");

        receipt.Customer = await this.context.Customers.FirstOrDefaultAsync(x => x.Id == receipt.CustomerId)
            ?? throw new ArgumentException("No such customer with this id!");

        var details = await this.context.ReceiptsDetails.Where(x => x.ReceiptId == receipt.Id).ToListAsync();
        foreach (var detail in details)
        {
            detail.Receipt = receipt;
            detail.Product = await this.context.Products.FirstOrDefaultAsync(x => x.Id == detail.ProductId)
                ?? throw new ArgumentException("No such product with this id!");
            detail.Product.Category = await this.context.ProductCategories.FirstOrDefaultAsync(x => x.Id == detail.Product.ProductCategoryId) ?? throw new ArgumentException("No such category!");
        }

        receipt.ReceiptDetails = details;
        return receipt;
    }

    public void Update(Receipt entity)
    {
        ArgumentNullException.ThrowIfNull(nameof(entity));
        bool validation = ValidateEntity(entity);
        if (validation)
        {
            _ = this.context.Receipts.Update(entity);
            _ = this.context.SaveChanges();
        }
    }
}
