using System.Globalization;
using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Business.Validation;
using Data.Entities;
using Data.Interfaces;

namespace Business.Services;

public class CustomerService : ICustomerService
{
    private readonly IUnitOfWork unitOfWork;

    private readonly IMapper mapper;

    public CustomerService(IUnitOfWork unt, IMapper mp)
    {
        this.mapper = mp;
        this.unitOfWork = unt;
    }

    public async Task<CustomerModel> AddAsync(CustomerModel model)
    {
        if (model is null)
        {
            throw new MarketException("The model itself was null!");
        }

        bool isValid = ValidateInput(model);
        if (!isValid)
        {
            throw new MarketException("Parameters inside model were null!");
        }

        if (DateTime.Compare(model.BirthDate, DateTime.Now) > 0 || DateTime.Compare(model.BirthDate, new DateTime(1820, 1, 1, 1, 1, 1, kind: DateTimeKind.Utc)) < 0 || model.DiscountValue < 0 || string.IsNullOrWhiteSpace(model.Surname))
        {
            throw new MarketException("Birth date is wrong!");
        }

        Customer entity = this.mapper.Map<Customer>(model);
        await this.unitOfWork.CustomerRepository.AddAsync(entity);
        await this.unitOfWork.SaveAsync();
        return this.mapper.Map<CustomerModel>(entity);
    }

    public async Task DeleteAsync(int modelId)
    {
        if (modelId < 0)
        {
            throw new MarketException("The id was negative!");
        }

        await this.unitOfWork.CustomerRepository.DeleteByIdAsync(modelId);
        await this.unitOfWork.SaveAsync();
    }

    public async Task<IEnumerable<CustomerModel>> GetAllAsync()
    {
        List<CustomerModel> result = new List<CustomerModel>();
        var collection = await this.unitOfWork.CustomerRepository.GetAllWithDetailsAsync();
        if (collection is null)
        {
            throw new MarketException("The collection was null or empty");
        }

        foreach (var item in collection)
        {
            result.Add(this.mapper.Map<CustomerModel>(item));
        }

        return result;
    }

    public async Task<CustomerModel> GetByIdAsync(int id)
    {
        if (id < 0)
        {
            throw new MarketException("The id was negative!");
        }

        var dbObject = await this.unitOfWork.CustomerRepository.GetByIdAsync(id);
        if (dbObject is null)
        {
            return null!;
        }

        return this.mapper.Map<CustomerModel>(dbObject);
    }

    public async Task<IEnumerable<CustomerModel>> GetCustomersByProductIdAsync(int productId)
    {
        if (productId < 0)
        {
            throw new MarketException("The product id was negative!");
        }

        // Get all customers with their receipts and details
        var customers = await this.unitOfWork.CustomerRepository.GetAllWithDetailsAsync();

        // Find customers who have at least one receipt with a detail for the given productId
        var result = customers
            .Where(c => c.Receipts != null && c.Receipts
                .Any(r => r.ReceiptDetails != null && r.ReceiptDetails.Any(rd => rd.ProductId == productId)))
            .Select(c => this.mapper.Map<CustomerModel>(c))
            .ToList();

        return result;
    }

    public async Task UpdateAsync(CustomerModel model)
    {
        if (model is null)
        {
            throw new MarketException("The model itself was null!");
        }

        bool isValid = ValidateInput(model);
        if (!isValid)
        {
            throw new MarketException("Parameters inside model were null!");
        }

        Customer entity = this.mapper.Map<Customer>(model);
        this.unitOfWork.CustomerRepository.Update(entity);
        await this.unitOfWork.SaveAsync();
    }

    private static bool ValidateInput(CustomerModel model)
    {
        var properties = model.GetType().GetProperties();
        foreach (var property in properties)
        {
            var propertyValue = property.GetValue(model, null);
            if (propertyValue is null || (propertyValue is string str && string.IsNullOrEmpty(str)) || (
                (propertyValue is DateTime && DateTime.Compare(Convert.ToDateTime(propertyValue, CultureInfo.InvariantCulture), new DateTime(1700, 1, 1, 0, 0, 0, kind: DateTimeKind.Utc)) < 0) ||
                (propertyValue is DateTime && DateTime.Compare(Convert.ToDateTime(propertyValue, CultureInfo.InvariantCulture), new DateTime(2049, 1, 1, 0, 0, 0, kind: DateTimeKind.Utc)) > 0)))
            {
                return false;
            }
        }

        return true;
    }
}
