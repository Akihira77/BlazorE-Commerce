using BlazorEcommerce.Server.Services.EmailService;
using BlazorEcommerce.Server.Services.Repositories.IRepositories;
using BlazorEcommerce.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace BlazorEcommerce.Server.Controllers;
[Route("api/v1/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IEmailSender _emailSender;

	public OrderController(IUnitOfWork unitOfWork, IEmailSender emailSender)
	{
		_unitOfWork = unitOfWork;
		_emailSender = emailSender;
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

	[HttpGet("invoice/{orderId}")]
	public async Task<bool> Invoice(int orderId)
	{
		var userEmail = _unitOfWork.Auth.GetUserEmail();
		var message = new Message(
					new string[] { userEmail },
					"order",
					orderId.ToString());

		await _emailSender.SendEmailAsync(message);
		return true;
	}
}
