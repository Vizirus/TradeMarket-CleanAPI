using Data.Interfaces;
using Data.Repositories;

namespace Data.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly TradeMarketDbContext context;

    public UnitOfWork(TradeMarketDbContext tradeMarketDb)
    {
        this.context = tradeMarketDb;
        this.PersonRepository = new PersonRepository(tradeMarketDb);
        this.ProductRepository = new ProductRepository(tradeMarketDb);
        this.CategoryRepository = new CategoryRepository(tradeMarketDb);
        this.ReceiptRepository = new ReceiptRepository(tradeMarketDb);
        this.ReceiptDetailRepository = new ReceiptDetailRepository(tradeMarketDb);
        this.CustomerRepository = new CustomerRepository(tradeMarketDb);
    }

    public ICustomerRepository CustomerRepository { get; }

    public IPersonRepository PersonRepository { get; }

    public IProductRepository ProductRepository { get; }

    public ICategoryRepository CategoryRepository { get; }

    public IReceiptRepository ReceiptRepository { get; }

    public IReceiptDetailRepository ReceiptDetailRepository { get; }

    public async Task SaveAsync()
    {
        _ = await this.context.SaveChangesAsync();
    }
}
