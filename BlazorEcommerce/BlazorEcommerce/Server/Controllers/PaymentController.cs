using BlazorEcommerce.Server.Services.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlazorEcommerce.Server.Controllers;
[Route("api/v1/[controller]")]
[ApiController]
public class PaymentController : ControllerBase
{
	private readonly IUnitOfWork _unitOfWork;

	public PaymentController(IUnitOfWork unitOfWork)
    {
		_unitOfWork = unitOfWork;
	}

	[HttpPost("checkout"), Authorize]
	public async Task<ActionResult<string>> CreateCheckoutSession()
	{
		var session = await _unitOfWork.Payment.CreateCheckoutSession();
		return Ok(session.Url);
	}

	[HttpPost]
	public async Task<ActionResult<ServiceResponse<bool>>> FulfillOrder()
	{
		var request = await _unitOfWork.Payment.FulfillOrder(Request);
		var response = new ServiceResponse<bool>();
		if (request.Success)
		{
			if(request.Data > 0)
			{
				var userId = request.Data;
				var cartItems = await _unitOfWork.Cart.GetAll((c => c.UserId == userId));
				var cartProducts = await _unitOfWork.Cart.GetCartProducts(cartItems);
				var order = _unitOfWork.Order.PlaceOrder(cartProducts, userId);

				await _unitOfWork.Order.Add(order);

				_unitOfWork.Cart.RemoveRange(cartItems);
				await _unitOfWork.Save();
			}
			return Ok(response);
		} else
		{
			response.Message = request.Message;
			response.Success = false;
			return BadRequest(response);
		} 
	}
}
