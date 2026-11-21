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
    }

    public void Delete(ReceiptDetail entity)
    {
        _ = this.Context.ReceiptsDetails.Remove(entity);
    }

    public async Task DeleteByIdAsync(int id)
    {
        var detail = await this.Context.ReceiptsDetails.FirstOrDefaultAsync(x => x.Id == id);
        if (detail != null)
        {
            _ = this.Context.ReceiptsDetails.Remove(detail);
        }
    }

    public async Task<IEnumerable<ReceiptDetail>> GetAllAsync()
    {
        return await this.Context.ReceiptsDetails.AsNoTracking().ToListAsync();
    }

    public async Task<IEnumerable<ReceiptDetail>> GetAllWithDetailsAsync()
    {
        return await this.Context.ReceiptsDetails
            .Include(rd => rd.Receipt)
            .Include(rd => rd.Product)
                .ThenInclude(p => p!.Category)
            .ToListAsync();
    }

    public async Task<ReceiptDetail?> GetByIdAsync(int id)
    {
        var detail = await this.Context.ReceiptsDetails.FirstOrDefaultAsync(x => x.Id == id);
        return detail;
    }

    public void Update(ReceiptDetail entity)
    {
        var existing = this.Context.ReceiptsDetails
         .FirstOrDefault(c => c.Id == entity.Id);

        if (existing != null)
        {
            this.Context.Entry(existing).CurrentValues.SetValues(entity);
        }
    }
}
