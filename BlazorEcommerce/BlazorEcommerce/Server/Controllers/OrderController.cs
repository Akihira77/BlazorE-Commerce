using BlazorEcommerce.Server.Services.Repositories.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlazorEcommerce.Server.Controllers;
[Route("api/v1/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
	private readonly IUnitOfWork _unitOfWork;

	public OrderController(IUnitOfWork unitOfWork)
    {
		_unitOfWork = unitOfWork;
	}

	[HttpPost]
	public async Task<ActionResult<ServiceResponse<bool>>> PlaceOrder()
	{
		var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
		var cartItems = await _unitOfWork.Cart.GetAll((c => c.UserId == userId));
		var cartProducts = await _unitOfWork.Cart.GetCartProducts(cartItems);

		var order = _unitOfWork.Order.PlaceOrder(cartProducts);
		await _unitOfWork.Order.Add(order);

		_unitOfWork.Cart.RemoveRange(cartItems);
		await _unitOfWork.Save();

		var response = new ServiceResponse<bool>
		{
			Data = true,
			Message = "Placing order success"
		};

		return Ok(response);
	}

	[HttpGet]
	public async Task<ActionResult<ServiceResponse<IEnumerable<OrderOverviewDto>>>> GetOrders()
	{
		var result = await _unitOfWork.Order.GetOrders();
		var response = new ServiceResponse<IEnumerable<OrderOverviewDto>>
		{
			Data = result,
			Message = "Order list"
		};

		return Ok(response);
	}

	[HttpGet("{orderId}")]
	public async Task<ActionResult<ServiceResponse<OrderDetailsDto>>> GetOrderDetails(int orderId)
	{
		var result = await _unitOfWork.Order.GetOrderDetails(orderId);
		var response = new ServiceResponse<OrderDetailsDto>
		{
			Data = result,
			Message = "List order details"
		};

		return Ok(response);
	}
}
