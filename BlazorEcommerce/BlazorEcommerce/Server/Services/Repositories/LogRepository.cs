using BlazorEcommerce.Server.Data;
using BlazorEcommerce.Server.Services.Repositories.IRepositories;

namespace BlazorEcommerce.Server.Services.Repositories;

public class LogRepository : Repository<Logs>, ILogRepository
{
	public LogRepository(AppDbContext db) : base(db)
	{
	}
}
