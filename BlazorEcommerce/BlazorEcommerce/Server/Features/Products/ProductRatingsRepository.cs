using BlazorEcommerce.Server.Data;
using BlazorEcommerce.Server.Features.Base;

namespace BlazorEcommerce.Server.Features.Products;

public class ProductRatingsRepository : Repository<ProductRatings>, IProductRatingsRepository
{
    public ProductRatingsRepository(AppDbContext db) : base(db)
    {
    }}
