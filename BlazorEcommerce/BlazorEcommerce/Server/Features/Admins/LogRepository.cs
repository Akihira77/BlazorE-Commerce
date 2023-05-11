using BlazorEcommerce.Server.Data;
using BlazorEcommerce.Server.Features.Base;

namespace BlazorEcommerce.Server.Features.Admins;

public class LogRepository : Repository<Logs>, ILogRepository
{
    public LogRepository(AppDbContext db) : base(db)
    {
    }
}
