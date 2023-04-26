using BlazorEcommerce.Server.Services.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlazorEcommerce.Server.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class AddressController : ControllerBase
{
	private readonly IUnitOfWork _unitOfWork;

	public AddressController(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
	}

	[HttpGet]
	public async Task<ActionResult<ServiceResponse<Address>>> GetAddress(int? userId = null)
	{
		var response = new ServiceResponse<Address>();

		if(userId == null)
		{
			response.Data = await _unitOfWork.Address
							.GetFirstOrDefault((a => a.UserId == _unitOfWork.Auth.GetUserId()));
		} else
		{
			response.Data = await _unitOfWork.Address
							.GetFirstOrDefault((a => a.UserId == userId));
		}
		return Ok(response);
	}

	[HttpGet("get-address/{userId}")]
	public async Task<ActionResult<ServiceResponse<Address>>> GetAddressUser(int? userId = null)
	{
		var response = new ServiceResponse<Address>();

		if(userId == null)
		{
			response.Data = await _unitOfWork.Address
							.GetFirstOrDefault((a => a.UserId == _unitOfWork.Auth.GetUserId()));
		} else
		{
			response.Data = await _unitOfWork.Address
							.GetFirstOrDefault((a => a.UserId == userId));
		}
		return Ok(response);
	}

	[HttpPost]
	public async Task<ActionResult<ServiceResponse<Address>>> AddOrUpdateAddress(Address address)
	{
		var dbAddress = await _unitOfWork
								.Address
								.GetFirstOrDefault((a => a.Id == address.Id), track: false);

		var response = new ServiceResponse<Address>
		{
			Message = "adding or modify Address is failed"
		};

		if(dbAddress == null)
		{
			address.UserId = _unitOfWork.Auth.GetUserId();
			await _unitOfWork.Address.Add(address);
			response.Message = "Adding address success";
		} else
		{
			//dbAddress.FirstName = address.FirstName;
			//dbAddress.LastName = address.LastName;
			//dbAddress.State = address.State;
			//dbAddress.Country = address.Country;
			//dbAddress.City = address.City;
			//dbAddress.Zip = address.Zip;
			//dbAddress.Street = address.Street;
			_unitOfWork.Address.Update(address);
			response.Message = "Updating address success";
		}

		await _unitOfWork.SaveAsync();
		response.Data = address;

		return Ok(response);
	}
}