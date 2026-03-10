using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Business.Validation;
using Data.Entities;
using Data.Interfaces;

namespace Business.Services;

public class ProductService : IProductService
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    public async Task<ProductModel> AddAsync(ProductModel model)
    {
        if (model is null)
        {
            throw new MarketException("The model itself was null!");
        }

        if (string.IsNullOrWhiteSpace(model.ProductName))
        {
            throw new MarketException("Product name cannot be empty!");
        }

        if (model.Price <= 0)
        {
            throw new MarketException("Product price must be positive!");
        }

        bool isValid = ValidateInput(model);
        if (!isValid)
        {
            throw new MarketException("Parameters inside model were null!");
        }

        Product entity = this.mapper.Map<Product>(model);
        await this.unitOfWork.ProductRepository.AddAsync(entity);
        await this.unitOfWork.SaveAsync();
        return this.mapper.Map<ProductModel>(entity);
    }

    public async Task DeleteAsync(int modelId)
    {
        if (modelId < 0)
        {
            throw new MarketException("The id was negative!");
        }

        await this.unitOfWork.ProductRepository.DeleteByIdAsync(modelId);
        await this.unitOfWork.SaveAsync();
    }

    public async Task<IEnumerable<ProductModel>> GetAllAsync()
    {
        List<ProductModel> result = new List<ProductModel>();
        var collection = await this.unitOfWork.ProductRepository.GetAllWithDetailsAsync();
        if (collection is null || !collection.Any())
        {
            throw new MarketException("The collection was null or empty");
        }

        foreach (var item in collection)
        {
            result.Add(this.mapper.Map<ProductModel>(item));
        }

        return result;
    }

    public async Task<ProductModel> GetByIdAsync(int id)
    {
        if (id < 0)
        {
            throw new MarketException("The id was negative!");
        }

        var dbObject = await this.unitOfWork.ProductRepository.GetByIdWithDetailsAsync(id);
        if (dbObject is null)
        {
            throw new MarketException("No such product exists with such id");
        }

        return this.mapper.Map<ProductModel>(dbObject);
    }

    public async Task UpdateAsync(ProductModel model)
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

        Product entity = this.mapper.Map<Product>(model);
        this.unitOfWork.ProductRepository.Update(entity);
        await this.unitOfWork.SaveAsync();
    }

    public async Task<IEnumerable<CategoryModel>> GetAllProductCategoriesAsync()
    {
        var values = await this.unitOfWork.CategoryRepository.GetAllAsync();
        return values.Select(x => this.mapper.Map<CategoryModel>(x));
    }

    public async Task<IEnumerable<ProductModel>> GetByFilterAsync(FilterSearchModel filterSearch)
    {
        if (filterSearch is null)
        {
            throw new MarketException("The filter class was null!");
        }

        int? filterId = filterSearch.CategoryId;
        if (filterId.HasValue && filterId < 0)
        {
            throw new MarketException("The filter id was null!");
        }

        var allProducts = await this.unitOfWork.ProductRepository.GetAllWithDetailsAsync();
        var returnProducts = allProducts.Where(x =>
            (!filterId.HasValue || x.ProductCategoryId == filterId) &&
            (filterSearch.MinPrice == null || x.Price >= filterSearch.MinPrice) &&
            (filterSearch.MaxPrice == null || x.Price <= filterSearch.MaxPrice));

        if (!returnProducts.Any())
        {
            throw new MarketException("Couldn`t find any product with this category and price range!");
        }

        return returnProducts.Select(x => this.mapper.Map<ProductModel>(x));
    }

    public async Task AddCategoryAsync(CategoryModel categoryModel)
    {
        if (categoryModel is null)
        {
            throw new MarketException("The category model itself was null!");
        }

        bool isValid = ValidateCategoryInput(categoryModel);
        if (!isValid)
        {
            throw new MarketException("Parameters inside category model were null!");
        }

        var entity = this.mapper.Map<ProductCategory>(categoryModel);
        await this.unitOfWork.CategoryRepository.AddAsync(entity);
        await this.unitOfWork.SaveAsync();
    }

    public async Task UpdateCategoryAsync(CategoryModel categoryModel)
    {
        if (categoryModel is null)
        {
            throw new MarketException("The category model itself was null!");
        }

        bool isValid = ValidateCategoryInput(categoryModel);
        if (!isValid)
        {
            throw new MarketException("Parameters inside category model were null!");
        }

        var entity = this.mapper.Map<ProductCategory>(categoryModel);
        this.unitOfWork.CategoryRepository.Update(entity);
        await this.unitOfWork.SaveAsync();
    }

    public async Task RemoveCategoryAsync(int categoryId)
    {
        if (categoryId < 0)
        {
            throw new MarketException("The category id was negative!");
        }

        await this.unitOfWork.CategoryRepository.DeleteByIdAsync(categoryId);
        await this.unitOfWork.SaveAsync();
    }

    private static bool ValidateCategoryInput(CategoryModel model)
    {
        var properties = model.GetType().GetProperties();
        foreach (var property in properties)
        {
            var propertyValue = property.GetValue(model, null);
            if (propertyValue == null || (propertyValue is string str && string.IsNullOrEmpty(str)))
            {
                return false;
            }
        }

        return true;
    }

    private static bool ValidateInput(ProductModel model)
    {
        var properties = model.GetType().GetProperties();
        foreach (var property in properties)
        {
            var propertyValue = property.GetValue(model, null);
            if (propertyValue is null || (propertyValue is string str && string.IsNullOrEmpty(str) && property.Name != "CategoryName"))
            {
                return false;
            }
        }

        return true;
    }
}
