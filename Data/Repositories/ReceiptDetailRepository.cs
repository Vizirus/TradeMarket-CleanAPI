using Data.Data;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class ReceiptDetailRepository : AbstractRepository, IReceiptDetailRepository
{
    public ReceiptDetailRepository(TradeMarketDbContext context)
        : base(context)
    {
    }

    public async Task AddAsync(ReceiptDetail entity)
    {
        ArgumentNullException.ThrowIfNull(nameof(entity));
        bool validation = ValidateEntity(entity);
        if (validation)
        {
            _ = await this.context.ReceiptsDetails.AddAsync(entity);
            _ = await this.context.SaveChangesAsync();
        }
    }

    public void Delete(ReceiptDetail entity)
    {
        ArgumentNullException.ThrowIfNull(nameof(entity));
        bool validation = ValidateEntity(entity);
        if (validation)
        {
            _ = this.context.ReceiptsDetails.Remove(entity);
            _ = this.context.SaveChanges();
        }
    }

    public async Task DeleteByIdAsync(int id)
    {
        if (id >= 0)
        {
            var productCategory = await this.context.ReceiptsDetails.FirstOrDefaultAsync(x => x.Id == id);
            if (productCategory != null)
            {
                _ = this.context.ReceiptsDetails.Remove(productCategory);
                _ = await this.context.SaveChangesAsync();
                return;
            }

            throw new ArgumentException("The id of receipt detail is inncorect! Could find any receipt details!");
        }

        throw new ArgumentException("The id of receipt detail is inncorect! Id was negative!");
    }

    public async Task<IEnumerable<ReceiptDetail>> GetAllAsync()
    {
        return await this.context.ReceiptsDetails.ToListAsync();
    }

    public async Task<IEnumerable<ReceiptDetail>> GetAllWithDetailsAsync()
    {
        var details = await this.context.ReceiptsDetails.ToListAsync();
        foreach (var detail in details)
        {
            detail.Receipt = await this.context.Receipts.FirstOrDefaultAsync(x => x.Id == detail.ReceiptId)
                ?? throw new ArgumentException("No such receipt with this id!");
            detail.Product = await this.context.Products.FirstOrDefaultAsync(x => x.Id == detail.ProductId)
                ?? throw new ArgumentException("No such product with this id!");
            detail.Product.Category = await this.context.ProductCategories.FirstOrDefaultAsync(x => x.Id == detail.Product.ProductCategoryId) ?? throw new ArgumentException("No such category!");
        }

        return details;
    }

    public async Task<ReceiptDetail?> GetByIdAsync(int id)
    {
        if (id >= 0)
        {
            var category = await this.context.ReceiptsDetails.FirstOrDefaultAsync(x => x.Id == id);
            if (category is null)
            {
                throw new ArgumentException("The id of receipt detail is inncorect! Could find any receipt details!");
            }

            return category;
        }

        throw new ArgumentException("The id of receipt detail is inncorect! Id was negative!");
    }

    public void Update(ReceiptDetail entity)
    {
        ArgumentNullException.ThrowIfNull(nameof(entity));
        bool validation = ValidateEntity(entity);
        if (validation)
        {
            _ = this.context.ReceiptsDetails.Update(entity);
            _ = this.context.SaveChanges();
        }
    }
}
