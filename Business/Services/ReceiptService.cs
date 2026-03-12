using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Business.Validation;
using Data.Entities;
using Data.Interfaces;

namespace Business.Services;

public class ReceiptService : IReceiptService
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    public ReceiptService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    public async Task<ReceiptModel> AddAsync(ReceiptModel model)
    {
        if (model is null)
        {
            throw new MarketException("The model itself was null !");
        }

        bool isValid = ValidateInput(model);
        if (!isValid)
        {
            throw new MarketException("Parameters inside model were null!");
        }

        var entity = this.mapper.Map<Receipt>(model);
        await this.unitOfWork.ReceiptRepository.AddAsync(entity);
        await this.unitOfWork.SaveAsync();
        return this.mapper.Map<ReceiptModel>(entity);
    }

    public async Task DeleteAsync(int modelId)
    {
        if (modelId < 0)
        {
            throw new MarketException("The id was negative");
        }

        var dbObject = await this.unitOfWork.ReceiptRepository.GetByIdWithDetailsAsync(modelId);
        if (dbObject!.ReceiptDetails is not null)
        {
            foreach (var details in dbObject.ReceiptDetails)
            {
                this.unitOfWork.ReceiptDetailRepository.Delete(details);
            }
        }

        if (dbObject is null)
        {
            return;
        }

        await this.unitOfWork.ReceiptRepository.DeleteByIdAsync(modelId);
        await this.unitOfWork.SaveAsync();
    }

    public async Task<IEnumerable<ReceiptModel>> GetAllAsync()
    {
        var collection = await this.unitOfWork.ReceiptRepository.GetAllWithDetailsAsync();
        if (collection is null || !collection.Any())
        {
            throw new MarketException("The collection was null or empty");
        }

        return collection.Select(x => this.mapper.Map<ReceiptModel>(x));
    }

    public async Task<ReceiptModel> GetByIdAsync(int id)
    {
        if (id < 0)
        {
            throw new MarketException("The id was negative");
        }

        var dbObject = await this.unitOfWork.ReceiptRepository.GetByIdWithDetailsAsync(id);

        return this.mapper.Map<ReceiptModel>(dbObject);
    }

    public async Task UpdateAsync(ReceiptModel model)
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

        var entity = this.mapper.Map<Receipt>(model);
        this.unitOfWork.ReceiptRepository.Update(entity);
        await this.unitOfWork.SaveAsync();
    }

    public async Task<IEnumerable<ReceiptDetailModel>> GetReceiptDetailsAsync(int receiptId)
    {
        if (receiptId < 0)
        {
            throw new MarketException("The id was negative!");
        }

        var receipt = await this.unitOfWork.ReceiptRepository.GetByIdWithDetailsAsync(receiptId);
        if (receipt is null)
        {
            throw new MarketException("No such receipt exists with such id");
        }

        if (receipt.ReceiptDetails == null || !receipt.ReceiptDetails.Any())
        {
            throw new MarketException("No receipt details found for this receipt");
        }

        return receipt.ReceiptDetails.Select(d => this.mapper.Map<ReceiptDetailModel>(d));
    }

    public async Task<decimal> ToPayAsync(int receiptId)
    {
        decimal totalsum = 0;

        if (receiptId < 0)
        {
            throw new MarketException("The id was negative!");
        }

        var dbObject = await this.unitOfWork.ReceiptRepository.GetByIdWithDetailsAsync(receiptId);
        if (dbObject is null)
        {
            throw new MarketException("No such receipt exists with such id");
        }

        if (dbObject.ReceiptDetails is null)
        {
            return 0;
        }

        foreach (var details in dbObject.ReceiptDetails ?? throw new MarketException("For some reasons Receipt details were null"))
        {
            totalsum += details.UnitPrice * details.Quantity;
        }

        var customer = await this.unitOfWork.CustomerRepository.GetByIdWithDetailsAsync(dbObject.CustomerId);
        if (customer != null && customer.DiscountValue > 0)
        {
            totalsum = totalsum * ((1 - customer.DiscountValue) / 100m);
        }

        return totalsum;
    }

    public async Task CheckOutAsync(int receiptId)
    {
        if (receiptId < 0)
        {
            throw new MarketException("The id was negative!");
        }

        var dbObject = await this.unitOfWork.ReceiptRepository.GetByIdWithDetailsAsync(receiptId);
        if (dbObject is null)
        {
            throw new MarketException("No such receipt exists with such id");
        }

        dbObject.IsCheckedOut = true;
        await this.UpdateAsync(this.mapper.Map<ReceiptModel>(dbObject));
    }

    public async Task<IEnumerable<ReceiptModel>> GetReceiptsByPeriodAsync(DateTime startDate, DateTime endDate)
    {
        if (startDate > endDate)
        {
            throw new MarketException("Start date must be before or equal to end date.");
        }

        var receipts = await this.unitOfWork.ReceiptRepository.GetAllWithDetailsAsync();
        var filtered = receipts.Where(r => r.OperationDate >= startDate && r.OperationDate <= endDate).ToList();

        return filtered.Select(r => this.mapper.Map<ReceiptModel>(r));
    }

    public async Task AddProductAsync(int productId, int receiptId, int quantity)
    {
        if (productId < 0 || receiptId < 0 || quantity <= 0)
        {
            throw new MarketException("Invalid ProductId, ReceiptId, or Quantity.");
        }

        var receipt = await this.unitOfWork.ReceiptRepository.GetByIdWithDetailsAsync(receiptId);
        if (receipt is null)
        {
            throw new MarketException("No such receipt exists with such id!");
        }

        var existingDetail = receipt.ReceiptDetails?.FirstOrDefault(rd => rd.ProductId == productId);
        if (existingDetail != null)
        {
            // Product already in receipt - just update quantity, no need to fetch product
            existingDetail.Quantity += quantity;
        }
        else
        {
            // Only fetch product when it's not already in the receipt
            var product = await this.unitOfWork.ProductRepository.GetByIdAsync(productId); // changed to GetByIdAsync
            if (product is null)
            {
                throw new MarketException("No such product exists with such id");
            }

            decimal discountUnitPrice = product.Price;
            if (receipt.Customer != null && receipt.Customer.DiscountValue > 0)
            {
                discountUnitPrice = product.Price * ((100 - receipt.Customer.DiscountValue) / 100m);
            }

            var newDetail = new ReceiptDetail
            {
                ProductId = productId,
                ReceiptId = receiptId,
                Quantity = quantity,
                UnitPrice = product.Price,
                DiscountUnitPrice = discountUnitPrice,
            };
            await this.unitOfWork.ReceiptDetailRepository.AddAsync(newDetail);
        }

        await this.unitOfWork.SaveAsync();
    }

    public async Task RemoveProductAsync(int productId, int receiptId, int quantity)
    {
        // Validate input
        if (productId < 0 || receiptId < 0 || quantity <= 0)
        {
            throw new MarketException("Invalid ProductId, ReceiptId, or Quantity.");
        }

        // Get receipt with details
        var receipt = await this.unitOfWork.ReceiptRepository.GetByIdWithDetailsAsync(receiptId);
        if (receipt is null)
        {
            throw new MarketException("No such receipt exists with such id!");
        }

        var detail = receipt.ReceiptDetails?.FirstOrDefault(rd => rd.ProductId == productId);
        if (detail is null)
        {
            return;
        }

        if (detail.Quantity < quantity)
        {
            throw new MarketException("Cannot remove more than exists in the receipt");
        }

        detail.Quantity -= quantity;
        if (detail.Quantity == 0)
        {
            this.unitOfWork.ReceiptDetailRepository.Delete(detail);
        }

        await this.unitOfWork.SaveAsync();
    }

    private static bool ValidateInput(ReceiptModel model)
    {
        var properties = model.GetType().GetProperties();
        foreach (var property in properties)
        {
            var propertyValue = property.GetValue(model, null);
            if (propertyValue == null)
            {
                return false;
            }
        }

        return true;
    }
}
