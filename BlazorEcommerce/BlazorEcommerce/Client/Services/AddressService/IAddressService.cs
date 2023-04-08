namespace BlazorEcommerce.Client.Services.AddressService;

public interface IAddressService
{
	Task<Address> GetAddress(int? userId = null);
	Task<Address> AddOrUpdateAddress(Address address);
}
