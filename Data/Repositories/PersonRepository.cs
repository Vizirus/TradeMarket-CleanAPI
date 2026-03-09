using Data.Data;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class PersonRepository : AbstractRepository, IPersonRepository
{
    public PersonRepository(TradeMarketDbContext context)
        : base(context)
    {
    }

    public async Task AddAsync(Person entity)
    {
        ArgumentNullException.ThrowIfNull(nameof(entity));
        bool validation = ValidateEntity(entity);
        if (validation)
        {
            _ = await this.context.Persons.AddAsync(entity);
            _ = await this.context.SaveChangesAsync();
        }
    }

    public void Delete(Person entity)
    {
        ArgumentNullException.ThrowIfNull(nameof(entity));
        bool validation = ValidateEntity(entity);
        if (validation)
        {
            _ = this.context.Persons.Remove(entity);
            _ = this.context.SaveChanges();
        }
    }

    public async Task DeleteByIdAsync(int id)
    {
        if (id >= 0)
        {
            var productCategory = await this.context.Persons.FirstOrDefaultAsync(x => x.Id == id);
            if (productCategory != null)
            {
                _ = this.context.Persons.Remove(productCategory);
                _ = await this.context.SaveChangesAsync();
                return;
            }

            throw new ArgumentException("The id of person is inncorect! Could find any persons!");
        }

        throw new ArgumentException("The id of person is inncorect! Id was negative!");
    }

    public async Task<IEnumerable<Person>> GetAllAsync()
    {
        return await this.context.Persons.ToListAsync();
    }

    public async Task<Person?> GetByIdAsync(int id)
    {
        if (id >= 0)
        {
            var category = await this.context.Persons.FirstOrDefaultAsync(x => x.Id == id);
            if (category is null)
            {
                throw new ArgumentException("The id of person is inncorect! Could find any personss!");
            }

            return category;
        }

        throw new ArgumentException("The id of person is inncorect! Id was negative!");
    }

    public void Update(Person entity)
    {
        ArgumentNullException.ThrowIfNull(nameof(entity));
        bool validation = ValidateEntity(entity);
        if (validation)
        {
            _ = this.context.Persons.Update(entity);
            _ = this.context.SaveChanges();
        }
    }
}
