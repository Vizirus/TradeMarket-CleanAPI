using Data.Data;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;
public class ProductRepository : AbstractRepository, IProductRepository
{
    public ProductRepository(TradeMarketDbContext context)
        : base(context)
    {
    }

    public async Task AddAsync(Product entity)
    {
        _ = await this.Context.Products.AddAsync(entity);
    }

    public void Delete(Product entity)
    {
        _ = this.Context.Products.Remove(entity);
    }

    public async Task DeleteByIdAsync(int id)
    {
        var product = await this.Context.Products.FirstOrDefaultAsync(x => x.Id == id);
        if (product != null)
        {
            _ = this.Context.Products.Remove(product);
            _ = await this.Context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await this.Context.Products.AsNoTracking().ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetAllWithDetailsAsync()
    {
        List<Product> returnList = new ();
        foreach (var i in this.Context.Products)
        {
            i.Category = await this.Context.ProductCategories.FirstOrDefaultAsync(x => x.Id == i.ProductCategoryId) ?? throw new ArgumentException("No such product category with this id!");
            var receipts = await this.Context.ReceiptsDetails.Where(x => x.ProductId == i.Id).ToListAsync();
            foreach (var receipt in receipts)
            {
                receipt.Product = i;
            }

            returnList.Add(i);
        }

        return returnList;
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        var productCollection = await this.Context.Products.ToListAsync();
        var product = productCollection.FirstOrDefault(x => x.Id == id);

        return product;
    }

    public async Task<Product?> GetByIdWithDetailsAsync(int id)
    {
        var i = await this.Context.Products.FirstOrDefaultAsync(x => x.Id == id) ?? throw new ArgumentException("No such product with this id!");
        i.Category = await this.Context.ProductCategories.FirstOrDefaultAsync(x => x.Id == i.ProductCategoryId) ?? throw new ArgumentException("No such product category with this id!");
        var receipts = await this.Context.ReceiptsDetails.Where(x => x.ProductId == i.Id).ToListAsync();
        foreach (var receipt in receipts)
        {
            receipt.Product = i;
        }

        return i;
    }

    public void Update(Product entity)
    {
        var existing = this.Context.Products
         .FirstOrDefault(c => c.Id == entity.Id);

        if (existing != null)
        {
            this.Context.Entry(existing).CurrentValues.SetValues(entity);
        }
    }
}
