using BlazorEcommerce.Server.Data;

namespace BlazorEcommerce.Server.DbInitializer;

public class DbInitializer : IDbInitializer
{
	private readonly AppDbContext _db;

	public DbInitializer(
        AppDbContext db
        )
    {
		_db = db;
	}
    public void Initialize()
	{
		try
		{
			if (_db.Database.GetPendingMigrations().Count() > 0)
			{
				_db.Database.Migrate();
			}
		} catch(Exception ex)
		{}
	}
}
