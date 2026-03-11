using System.Reflection;
using Data.Data;
using Data.Entities;

namespace Data.Repositories;
public class AbstractRepository
{
    public AbstractRepository(TradeMarketDbContext context)
    {
        this.Context = context;
    }

    protected TradeMarketDbContext Context { get; }

    protected static bool ValidateEntity(BaseEntity entity)
    {
        var properties = entity != null ? entity.GetType().GetProperties() : throw new ArgumentNullException(nameof(entity), "The argument was null!");
        if (properties.Length > 0)
        {
            foreach (var property in properties)
            {
                var value = property.GetValue(entity);
                var hasColumnAttribute = property.GetCustomAttribute<System.ComponentModel.DataAnnotations.Schema.ColumnAttribute>() != null;

                if (value is null && hasColumnAttribute)
                {
                    return false;
                }
            }
        }

        return true;
    }
}
