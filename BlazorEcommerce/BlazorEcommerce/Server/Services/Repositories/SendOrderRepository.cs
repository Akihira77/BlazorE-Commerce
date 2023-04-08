using BlazorEcommerce.Server.Data;
using BlazorEcommerce.Server.Services.Repositories.IRepositories;

namespace BlazorEcommerce.Server.Services.Repositories;

public class SendOrderRepository : Repository<SendOrder>, ISendOrderRepository
{
	public SendOrderRepository(AppDbContext db) : base(db)
	{
	}
}
