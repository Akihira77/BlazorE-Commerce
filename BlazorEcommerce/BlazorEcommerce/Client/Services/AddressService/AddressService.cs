﻿namespace BlazorEcommerce.Client.Services.AddressService;

public class AddressService : IAddressService
{
	private readonly HttpClient _http;

	public AddressService(HttpClient http)
    {
		_http = http;
	}
    public async Task<Address> AddOrUpdateAddress(Address address)
	{
		var response = await _http.PostAsJsonAsync("api/v1/address", address);

		return response
			.Content
			.ReadFromJsonAsync
			<ServiceResponse<Address>>().Result.Data;
	}

	public async Task<Address> GetAddress(int? userId = null)
	{
		ServiceResponse<Address> response;
		if(userId == null)
		{
			response = await _http.GetFromJsonAsync<ServiceResponse<Address>>("api/v1/address");
		} else
		{
			response = await _http.GetFromJsonAsync<ServiceResponse<Address>>($"api/v1/address/get-address/{userId}");
		} 
		return response.Data;
	}
}
