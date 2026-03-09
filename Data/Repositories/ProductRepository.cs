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
        ArgumentNullException.ThrowIfNull(nameof(entity));
        bool validation = ValidateEntity(entity);
        if (validation)
        {
            _ = await this.context.Products.AddAsync(entity);
            _ = await this.context.SaveChangesAsync();
        }
    }

    public void Delete(Product entity)
    {
        ArgumentNullException.ThrowIfNull(nameof(entity));
        bool validation = ValidateEntity(entity);
        if (validation)
        {
            _ = this.context.Products.Remove(entity);
            _ = this.context.SaveChanges();
        }
    }

    public async Task DeleteByIdAsync(int id)
    {
        if (id >= 0)
        {
            var productCategory = await this.context.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (productCategory != null)
            {
                _ = this.context.Products.Remove(productCategory);
                _ = await this.context.SaveChangesAsync();
                return;
            }

            throw new ArgumentException("The id of product is inncorect! Could find any products!");
        }

        throw new ArgumentException("The id of product is inncorect! Id was negative!");
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await this.context.Products.ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetAllWithDetailsAsync()
    {
        List<Product> returnList = new();
        foreach (var i in this.context.Products)
        {
            i.Category = await this.context.ProductCategories.FirstOrDefaultAsync(x => x.Id == i.ProductCategoryId) ?? throw new ArgumentException("No such product category with this id!");
            var receipts = await this.context.ReceiptsDetails.Where(x => x.ProductId == i.Id).ToListAsync();
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
        if (id >= 0)
        {
            var category = await this.context.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (category is null)
            {
                throw new ArgumentException("The id of product is inncorect! Could find any products!");
            }

            return category;
        }

        throw new ArgumentException("The id of product is inncorect! Id was negative!");
    }

    public async Task<Product?> GetByIdWithDetailsAsync(int id)
    {
        var i = await this.context.Products.FirstOrDefaultAsync(x => x.Id == id) ?? throw new ArgumentException("No such product category with this id!"); ;
        i.Category = await this.context.ProductCategories.FirstOrDefaultAsync(x => x.Id == i.ProductCategoryId) ?? throw new ArgumentException("No such product category with this id!");
        var receipts = await this.context.ReceiptsDetails.Where(x => x.ProductId == i.Id).ToListAsync();
        foreach (var receipt in receipts)
        {
            receipt.Product = i;
        }

        return i;
    }

    public void Update(Product entity)
    {
        ArgumentNullException.ThrowIfNull(nameof(entity));
        bool validation = ValidateEntity(entity);
        if (validation)
        {
            _ = this.context.Products.Update(entity);
            _ = this.context.SaveChanges();
        }
    }
}
