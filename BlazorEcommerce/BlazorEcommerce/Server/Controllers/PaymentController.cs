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
	public async Task<string> CreateCheckoutSession()
	{
		var session = await _unitOfWork.Payment.CreateCheckoutSession();
		await _unitOfWork.OrderHeader.Add(new OrderHeader
		{
			UserId = _unitOfWork.Auth.GetUserId(),
			SessionId = session.Id,
		});
		await _unitOfWork.Save();
		return session.Url;
	}

	[HttpPost]
	public async Task<ActionResult<ServiceResponse<bool>>> FulfillOrder()
	{
		var request = await _unitOfWork.Payment.FulfillOrder(Request);
		var response = new ServiceResponse<bool>();
		if(request.Success)
		{
			if(request.Data > 0)
			{
				var user = await _unitOfWork.Auth.GetFirstOrDefault((u => u.Id == request.Data));
				var cartItems = await _unitOfWork.Cart.GetAll((c => c.UserId == user.Id));
				var cartProducts = await _unitOfWork.Cart.GetCartProducts(cartItems);

				foreach(var product in cartProducts)
				{
					var dbProduct = await _unitOfWork.Product.GetFirstOrDefault((p => p.Id == product.ProductId));
					if(dbProduct != null)
					{
						dbProduct.Stock -= product.Quantity;
					}
				}

				var order = _unitOfWork.Order.PlaceOrder(cartProducts, user.Id);

				await _unitOfWork.Order.Add(order);

				_unitOfWork.Cart.RemoveRange(cartItems);
				await _unitOfWork.Save();

				response.Message = "Order has been fulfilled";
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
