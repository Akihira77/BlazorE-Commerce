using BlazorEcommerce.Server.Data;
using BlazorEcommerce.Server.Services.Repositories.IRepositories;

namespace BlazorEcommerce.Server.Services.Repositories;

public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
{
	public OrderHeaderRepository(AppDbContext db) : base(db)
	{
	}
}