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
        _ = await this.Context.Persons.AddAsync(entity);
        _ = await this.Context.SaveChangesAsync();
    }

    public void Delete(Person entity)
    {
        _ = this.Context.Persons.Remove(entity);
        _ = this.Context.SaveChanges();
    }

    public async Task DeleteByIdAsync(int id)
    {
        var person = await this.Context.Persons.FirstOrDefaultAsync(x => x.Id == id);
        if (person != null)
        {
            _ = this.Context.Persons.Remove(person);
            _ = await this.Context.SaveChangesAsync();
            return;
        }

        throw new ArgumentException("The id of person is inncorect! Could find any persons!");
    }

    public async Task<IEnumerable<Person>> GetAllAsync()
    {
        return await this.Context.Persons.ToListAsync();
    }

    public async Task<Person?> GetByIdAsync(int id)
    {
        var person = await this.Context.Persons.FirstOrDefaultAsync(x => x.Id == id);
        if (person is null)
        {
            throw new ArgumentException("The id of person is inncorect! Could find any persons!");
        }

        return person;
    }

    public void Update(Person entity)
    {
        _ = this.Context.Persons.Update(entity);
        _ = this.Context.SaveChanges();
    }
}
