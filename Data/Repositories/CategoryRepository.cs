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
        ArgumentNullException.ThrowIfNull(nameof(entity));
        bool validation = ValidateEntity(entity);
        if (validation)
        {
            _ = this.context.ProductCategories.Remove(entity);
            _ = this.context.SaveChanges();
        }
    }

    public async Task DeleteByIdAsync(int id)
    {
        if (id >= 0)
        {
            ProductCategory? productCategory = await this.context.ProductCategories.FirstOrDefaultAsync(x => x.Id == id);
            if (productCategory != null)
            {
                _ = this.context.ProductCategories.Remove(productCategory);
                _ = await this.context.SaveChangesAsync();
                return;
            }

            throw new ArgumentException("The id of category is inncorect! Could find any category!");
        }

        throw new ArgumentException("The id of category is inncorect! Id was negative!");
    }

    public async Task<IEnumerable<ProductCategory>> GetAllAsync()
    {
        return await this.context.ProductCategories.ToListAsync();
    }

    public async Task<ProductCategory?> GetByIdAsync(int id)
    {
        if (id >= 0)
        {
            var category = await this.context.ProductCategories.FirstOrDefaultAsync(x => x.Id == id);
            if (category is null)
            {
                throw new ArgumentException("The id of category is inncorect! Could find any category!");
            }

            return category;
        }

        throw new ArgumentException("The id of category is inncorect! Id was negative!");
    }

    public void Update(ProductCategory entity)
    {
        ArgumentNullException.ThrowIfNull(nameof(entity));
        bool validation = ValidateEntity(entity);
        if (validation)
        {
            _ = this.context.ProductCategories.Update(entity);
            _ = this.context.SaveChanges();
        }
    }
}
