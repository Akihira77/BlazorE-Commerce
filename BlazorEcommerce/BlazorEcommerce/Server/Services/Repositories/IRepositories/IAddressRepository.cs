namespace BlazorEcommerce.Server.Services.Repositories.IRepositories;

public interface IAddressRepository : IRepository<Address>
{
	void Update(Address address);
}
