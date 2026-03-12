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
        _ = await this.Context.Receipts.AddAsync(entity);
    }

    public void Delete(Receipt entity)
    {
        _ = this.Context.Receipts.Remove(entity);
    }

    public async Task DeleteByIdAsync(int id) // fixed: added async
    {
        var receipt = await this.Context.Receipts.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        if (receipt != null)
        {
            _ = this.Context.Receipts.Remove(receipt);
            _ = await this.Context.SaveChangesAsync();  // fixed: added await
        }
    }

    public async Task<IEnumerable<Receipt>> GetAllAsync()
    {
        return await this.Context.Receipts.ToListAsync();
    }

    public async Task<IEnumerable<Receipt>> GetAllWithDetailsAsync()
    {
        return await this.Context.Receipts
            .Include(r => r.Customer)
            .Include(r => r.ReceiptDetails!)
                .ThenInclude(rd => rd.Product)
                    .ThenInclude(p => p.Category)
            .ToListAsync();
    }

    public async Task<Receipt?> GetByIdAsync(int id)
    {
        var receipt = await this.Context.Receipts.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        return receipt;
    }

    // S4457 fix: public wrapper validates synchronously, private core does async work
    public Task<Receipt?> GetByIdWithDetailsAsync(int id)
    {
        if (id < 0)
        {
            throw new ArgumentException("The id of receipt is incorrect! Id was negative!");
        }

        return this.GetByIdWithDetailsCoreAsync(id);
    }

    public void Update(Receipt entity)
    {
        var existing = this.Context.Receipts
            .FirstOrDefault(c => c.Id == entity.Id);

        if (existing != null)
        {
            this.Context.Entry(existing).CurrentValues.SetValues(entity);
        }
    }

    public async Task<Receipt?> GetByIdWithDetailsCoreAsync(int id)
    {
        return await this.Context.Receipts
            .Include(r => r.Customer)
            .Include(r => r.ReceiptDetails!)
                .ThenInclude(rd => rd.Product)
                    .ThenInclude(p => p.Category)
            .FirstOrDefaultAsync(r => r.Id == id);
    }
}
