using BlazorEcommerce.Server.Data;
using BlazorEcommerce.Server.Services.Repositories.IRepositories;

namespace BlazorEcommerce.Server.Services.Repositories;

public class AddressRepository : Repository<Address>, IAddressRepository
{
	private readonly AppDbContext _db;

	public AddressRepository(AppDbContext db) : base(db)
	{
		_db = db;
	}

	public void Update(Address address)
	{
		_db.Update(address);
	}
}
