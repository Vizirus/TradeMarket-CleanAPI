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
    }

    public void Delete(Person entity)
    {
        _ = this.Context.Persons.Remove(entity);
    }

    public async Task DeleteByIdAsync(int id)
    {
        var person = await this.Context.Persons.FirstOrDefaultAsync(x => x.Id == id);
        if (person != null)
        {
            _ = this.Context.Persons.Remove(person);
            _ = await this.Context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Person>> GetAllAsync()
    {
        return await this.Context.Persons.ToListAsync();
    }

    public async Task<Person?> GetByIdAsync(int id)
    {
        var person = await this.Context.Persons.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        return person;
    }

    public void Update(Person entity)
    {
        var existing = this.Context.Persons
          .FirstOrDefault(c => c.Id == entity.Id);

        if (existing != null)
        {
            this.Context.Entry(existing).CurrentValues.SetValues(entity);
        }
    }
}
