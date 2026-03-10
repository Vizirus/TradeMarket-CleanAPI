using AutoMapper;
using Business.Models;
using Data.Entities;

namespace Business;

public class AutomapperProfile : Profile
{
    public AutomapperProfile()
    {
        // Customer <-> CustomerModel
        _ = this.CreateMap<Customer, CustomerModel>()
            .ForMember(cm => cm.Id, c => c.MapFrom(x => x.Id))
            .ForMember(cm => cm.Name, c => c.MapFrom(x => x.Person != null ? x.Person.Name : string.Empty))
            .ForMember(cm => cm.Surname, c => c.MapFrom(x => x.Person != null ? x.Person.Surname : string.Empty))
            .ForMember(cm => cm.BirthDate, c => c.MapFrom(x => x.Person != null ? x.Person.BirthDate : default))
            .ForMember(cm => cm.DiscountValue, c => c.MapFrom(x => x.DiscountValue))
            .ForMember(cm => cm.ReceiptsIds, c => c.MapFrom(x => x.Receipts != null ? x.Receipts.Select(r => r.Id) : new List<int>()))
            //.ForMember(c => c.Id, cm => cm.MapFrom(x => x.PersonId))
            .ReverseMap()
            .ForMember(c => c.Id, opt => opt.MapFrom(cm => cm.Id))
            .ForMember(c => c.PersonId, opt => opt.MapFrom(cm => cm.Id))
            .ForMember(c => c.Person, opt => opt.MapFrom(cm => new Person
            {
                Id = cm.Id,
                Name = cm.Name,
                Surname = cm.Surname,
                BirthDate = cm.BirthDate
            }))
            .ForMember(c => c.Receipts, opt => opt.Ignore());

        // Receipt <-> ReceiptModel
        _ = this.CreateMap<Receipt, ReceiptModel>()
           .ForMember(rm => rm.Id, r => r.MapFrom(x => x.Id))
           .ForMember(rm => rm.CustomerId, r => r.MapFrom(x => x.CustomerId))
           .ForMember(rm => rm.IsCheckedOut, r => r.MapFrom(x => x.IsCheckedOut))
           .ForMember(rm => rm.OperationDate, r => r.MapFrom(x => x.OperationDate))
           .ForMember(rm => rm.ReceiptDetailsIds, r => r.MapFrom(x => x.ReceiptDetails != null ? x.ReceiptDetails.Select(d => d.Id) : new List<int>()))
           .ReverseMap()
           .ForMember(r => r.Customer, opt => opt.Ignore())
           .ForMember(r => r.ReceiptDetails, opt => opt.Ignore());

        // Product <-> ProductModel
        _ = this.CreateMap<Product, ProductModel>()
            .ForMember(pm => pm.Id, p => p.MapFrom(x => x.Id))
            .ForMember(pm => pm.ProductName, p => p.MapFrom(x => x.ProductName))
            .ForMember(pm => pm.Price, p => p.MapFrom(x => x.Price))
            .ForMember(pm => pm.ProductCategoryId, p => p.MapFrom(x => x.ProductCategoryId))
            .ForMember(pm => pm.CategoryName, p => p.MapFrom(x => x.Category != null ? x.Category.CategoryName : string.Empty))
            .ForMember(pm => pm.ReceiptDetailIds, p => p.MapFrom(x => x.ReceiptDetails != null ? x.ReceiptDetails.Select(rd => rd.Id) : new List<int>()))
            .ReverseMap()
            .ForMember(p => p.Category, opt => opt.Ignore())
            .ForMember(p => p.ReceiptDetails, opt => opt.Ignore());

        // ReceiptDetail <-> ReceiptDetailModel
        _ = this.CreateMap<ReceiptDetail, ReceiptDetailModel>()
            .ForMember(rdm => rdm.Id, rd => rd.MapFrom(x => x.Id))
            .ForMember(rdm => rdm.ReceiptId, rd => rd.MapFrom(x => x.ReceiptId))
            .ForMember(rdm => rdm.ProductId, rd => rd.MapFrom(x => x.ProductId))
            .ForMember(rdm => rdm.Quantity, rd => rd.MapFrom(x => x.Quantity))
            .ForMember(rdm => rdm.UnitPrice, rd => rd.MapFrom(x => x.UnitPrice))
            .ForMember(rdm => rdm.DiscountUnitPrice, rd => rd.MapFrom(x => x.DiscountUnitPrice))
            .ReverseMap();

        // ProductCategory <-> CategoryModel
        _ = this.CreateMap<ProductCategory, CategoryModel>()
            .ForMember(cm => cm.Id, c => c.MapFrom(x => x.Id))
            .ForMember(cm => cm.Name, c => c.MapFrom(x => x.CategoryName))
            .ForMember(cm => cm.ProductIds, c => c.MapFrom(x => x.Products != null ? x.Products.Select(p => p.Id) : new List<int>()))
            .ReverseMap()
            .ForMember(pc => pc.Products, opt => opt.Ignore());
    }
}
