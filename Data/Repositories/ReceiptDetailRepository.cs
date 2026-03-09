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
        _ = await this.Context.ReceiptsDetails.AddAsync(entity);
        _ = await this.Context.SaveChangesAsync();
    }

    public void Delete(ReceiptDetail entity)
    {
        _ = this.Context.ReceiptsDetails.Remove(entity);
        _ = this.Context.SaveChanges();
    }

    public async Task DeleteByIdAsync(int id)
    {
        var detail = await this.Context.ReceiptsDetails.FirstOrDefaultAsync(x => x.Id == id);
        if (detail != null)
        {
            _ = this.Context.ReceiptsDetails.Remove(detail);
            _ = await this.Context.SaveChangesAsync();
            return;
        }

        throw new ArgumentException("The id of receipt detail is incorrect! Could not find any receipt details!");
    }

    public async Task<IEnumerable<ReceiptDetail>> GetAllAsync()
    {
        return await this.Context.ReceiptsDetails.ToListAsync();
    }

    public async Task<IEnumerable<ReceiptDetail>> GetAllWithDetailsAsync()
    {
        var details = await this.Context.ReceiptsDetails.ToListAsync();
        foreach (var detail in details)
        {
            detail.Receipt = await this.Context.Receipts.FirstOrDefaultAsync(x => x.Id == detail.ReceiptId)
                ?? throw new ArgumentException("No such receipt with this id!");
            detail.Product = await this.Context.Products.FirstOrDefaultAsync(x => x.Id == detail.ProductId)
                ?? throw new ArgumentException("No such product with this id!");
            detail.Product.Category = await this.Context.ProductCategories.FirstOrDefaultAsync(x => x.Id == detail.Product.ProductCategoryId)
                ?? throw new ArgumentException("No such category!");
        }

        return details;
    }

    public async Task<ReceiptDetail?> GetByIdAsync(int id)
    {
        var detail = await this.Context.ReceiptsDetails.FirstOrDefaultAsync(x => x.Id == id);
        if (detail is null)
        {
            throw new ArgumentException("The id of receipt detail is incorrect! Could not find any receipt details!");
        }

        return detail;
    }

    public void Update(ReceiptDetail entity)
    {
        _ = this.Context.ReceiptsDetails.Update(entity);
        _ = this.Context.SaveChanges();
    }
}
