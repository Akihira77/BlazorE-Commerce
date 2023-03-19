using BlazorEcommerce.Server.Services.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;

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

	[HttpGet]
	public async Task<ActionResult<ServiceResponse<IEnumerable<OrderOverviewDto>>>> GetOrders()
	{
		var result = await _unitOfWork.Order.GetOrders();
		var response = new ServiceResponse<IEnumerable<OrderOverviewDto>>
		{
			Data = result ?? new List<OrderOverviewDto>(),
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
