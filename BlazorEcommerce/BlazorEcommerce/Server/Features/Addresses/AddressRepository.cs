using BlazorEcommerce.Server.Data;
using BlazorEcommerce.Server.Features.Base;

namespace BlazorEcommerce.Server.Features.Addresses;

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