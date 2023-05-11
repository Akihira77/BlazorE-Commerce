using BlazorEcommerce.Server.Data;
using BlazorEcommerce.Server.Features.Base;

namespace BlazorEcommerce.Server.Features.Orders;

public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
{
    public OrderHeaderRepository(AppDbContext db) : base(db)
    {
    }
}