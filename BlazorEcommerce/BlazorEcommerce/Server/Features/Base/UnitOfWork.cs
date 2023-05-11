using BlazorEcommerce.Server.Data;
using BlazorEcommerce.Server.Features.Addresses;
using BlazorEcommerce.Server.Features.Admins;
using BlazorEcommerce.Server.Features.Auths;
using BlazorEcommerce.Server.Features.Carts;
using BlazorEcommerce.Server.Features.Orders;
using BlazorEcommerce.Server.Features.Payments;
using BlazorEcommerce.Server.Features.Products;

namespace BlazorEcommerce.Server.Features.Base;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _db;

    public IProductRepository Product { get; private set; }
    public ICategoryRepository Category { get; private set; }
    public ICartRepository Cart { get; private set; }
    public IAuthRepository Auth { get; private set; }
    public IOrderRepository Order { get; private set; }
    public IPaymentRepository Payment { get; private set; }
    public IAddressRepository Address { get; private set; }
    public IProductTypeRepository ProductType { get; private set; }
    public IProductVariantRepository ProductVariant { get; private set; }
    public IOrderHeaderRepository OrderHeader { get; private set; }
    public IProductRatingsRepository ProductRatings { get; private set; }
    public ILogRepository Log { get; private set; }
    public IImagesRepository Images { get; private set; }

    public UnitOfWork(AppDbContext db
        , IConfiguration configuration
        , IHttpContextAccessor httpContextAccessor)
    {
        _db = db;
        Product = new ProductRepository(_db, httpContextAccessor);
        Category = new CategoryRepository(_db);
        Auth = new AuthRepository(_db, configuration, httpContextAccessor);
        Cart = new CartRepository(_db, Auth);
        Order = new OrderRepository(_db, Auth);
        Payment = new PaymentRepository(Cart, Auth);
        Address = new AddressRepository(_db);
        ProductType = new ProductTypeRepository(_db);
        ProductVariant = new ProductVariantRepository(_db);
        OrderHeader = new OrderHeaderRepository(_db);
        ProductRatings = new ProductRatingsRepository(_db);
        Log = new LogRepository(_db);
        Images = new ImagesRepository(_db);
    }

    public void Save() => _db.SaveChanges();

    public async Task SaveAsync() => await _db.SaveChangesAsync();
}