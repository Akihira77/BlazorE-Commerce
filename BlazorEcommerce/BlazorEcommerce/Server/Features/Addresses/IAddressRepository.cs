using BlazorEcommerce.Server.Features.Base;

namespace BlazorEcommerce.Server.Features.Addresses;

public interface IAddressRepository : IRepository<Address>
{
    void Update(Address address);
}