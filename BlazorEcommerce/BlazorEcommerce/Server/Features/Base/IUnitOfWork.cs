using BlazorEcommerce.Server.Features.Addresses;
using BlazorEcommerce.Server.Features.Admins;
using BlazorEcommerce.Server.Features.Auths;
using BlazorEcommerce.Server.Features.Carts;
using BlazorEcommerce.Server.Features.Orders;
using BlazorEcommerce.Server.Features.Payments;
using BlazorEcommerce.Server.Features.Products;

namespace BlazorEcommerce.Server.Features.Base;

public interface IUnitOfWork
{
    IProductRepository Product { get; }
    ICategoryRepository Category { get; }
    ICartRepository Cart { get; }
    IAuthRepository Auth { get; }
    IOrderRepository Order { get; }
    IPaymentRepository Payment { get; }
    IAddressRepository Address { get; }
    IProductTypeRepository ProductType { get; }
    IProductVariantRepository ProductVariant { get; }
    IOrderHeaderRepository OrderHeader { get; }
    IProductRatingsRepository ProductRatings { get; }
    ILogRepository Log { get; }
    IImagesRepository Images { get; }

    Task SaveAsync();

    void Save();
}