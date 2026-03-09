using Data.Data;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;
public class CategoryRepository : AbstractRepository, ICategoryRepository
{
    public CategoryRepository(TradeMarketDbContext context)
        : base(context)
    {
    }

    public async Task AddAsync(ProductCategory entity)
    {
        ArgumentNullException.ThrowIfNull(nameof(entity));
        bool validation = ValidateEntity(entity);
        if (validation)
        {
            _ = await this.context.ProductCategories.AddAsync(entity);
            _ = await this.context.SaveChangesAsync();
        }
    }

    public void Delete(ProductCategory entity)
    {
        _ = this.Context.ProductCategories.Remove(entity);
        _ = this.Context.SaveChanges();
    }

    public async Task DeleteByIdAsync(int id)
    {
        ProductCategory? productCategory = await this.Context.ProductCategories.FirstOrDefaultAsync(x => x.Id == id);
        if (productCategory != null)
        {
            _ = this.Context.ProductCategories.Remove(productCategory);
            _ = await this.Context.SaveChangesAsync();
            return;
        }

        throw new ArgumentException("The id of category is incorrect! Could not find any categories!");
    }

    public async Task<IEnumerable<ProductCategory>> GetAllAsync()
    {
        return await this.Context.ProductCategories.ToListAsync();
    }

    public async Task<ProductCategory?> GetByIdAsync(int id)
    {
        var category = await this.Context.ProductCategories.FirstOrDefaultAsync(x => x.Id == id);
        if (category is null)
        {
            throw new ArgumentException("The id of category is incorrect! Could not find any categories!");
        }

        return category; // removed unreachable throw after this
    }

    public void Update(ProductCategory entity)
    {
        _ = this.Context.ProductCategories.Update(entity);
        _ = this.Context.SaveChanges();
    }
}
