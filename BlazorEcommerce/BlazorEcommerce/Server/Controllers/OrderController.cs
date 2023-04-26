using BlazorEcommerce.Server.Services.EmailService;
using BlazorEcommerce.Server.Services.Repositories.IRepositories;
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

	[HttpGet("admin/{orderId}")]
	public async Task<ActionResult<ServiceResponse<OrderDetailsDto>>> AdminGetOrderDetails(int orderId)
	{
		var result = await _unitOfWork.Order.AdminGetOrderDetails(orderId);
		var response = new ServiceResponse<OrderDetailsDto>
		{
			Data = result,
			Message = $"Admin - List Order Details ID {orderId}"
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
					"invoice",
					orderId.ToString());

		await _emailSender.SendEmailAsync(message);
		return true;
	}

	[HttpGet("admin-orders")]
	public async Task<ActionResult<ServiceResponse<IEnumerable<OrderDto>>>> GetAdminOrders()
	{
		var orderDtos = new List<OrderDto>();
		var result = (await _unitOfWork.Order.GetAll())
					.OrderByDescending(o => o.OrderDate);

		foreach(var order in result)
		{
			orderDtos.Add(new OrderDto
			{
				User = await _unitOfWork.Auth.GetFirstOrDefault((u => u.Id == order.UserId), includeProperties: "Address"),
				Order = order
			});
		}
		return Ok(new ServiceResponse<IEnumerable<OrderDto>>
		{
			Data = orderDtos
		});
	}

	[HttpGet("admin/get-order/{orderId}")]
	public async Task<ActionResult<ServiceResponse<OrderOverviewDto>>> GetOrder(int orderId)
	{
		var order = await _unitOfWork.Order.GetFirstOrDefault((o => o.Id == orderId));

		var response = new ServiceResponse<OrderOverviewDto>();

		if(order == null)
		{
			response.Message = "Order does not exists";
			response.Success = false;

			return NotFound(response);
		}

		response.Data = new OrderOverviewDto
		{
			Id = order.Id,
			OrderStatus = order.OrderStatus,
		};

		return Ok(response);
	}

	[HttpPut("admin/update-order-status/{orderId}")]
	public async Task<ActionResult<ServiceResponse<IEnumerable<OrderOverviewDto>>>> UpdateOrderStatus(int orderId, [FromBody] int status)
	{
		var order = await _unitOfWork.Order
					.GetFirstOrDefault((o => o.Id == orderId));

		var response = new ServiceResponse<IEnumerable<OrderDto>>();

		if(order == null)
		{
			response.Message = "Order does not exists";
			response.Success = false;

			return NotFound(response);
		}

		order.OrderStatus = status;
		await _unitOfWork.SaveAsync();

		var orderDtos = new List<OrderDto>();
		var result = (await _unitOfWork.Order.GetAll())
					.OrderByDescending(o => o.OrderDate);

		foreach(var item in result)
		{
			orderDtos.Add(new OrderDto
			{
				User = await _unitOfWork.Auth.GetFirstOrDefault((u => u.Id == item.UserId), includeProperties: "Address"),
				Order = item
			});
		}
		response.Data = orderDtos;
		return Ok(response);
	}

	[HttpPost("admin/send-order")]
	public async Task SendOrder([FromBody] SendOrder sendOrder)
	{
		var order = await _unitOfWork.Order
				.GetFirstOrDefault((o => o.Id == sendOrder.OrderId));
		order.OrderStatus = 1;

		await _unitOfWork.SendOrder.Add(sendOrder);
		await _unitOfWork.SaveAsync();
	}

	[HttpDelete("cancel-order/{orderId}")]
	public async Task<ActionResult<ServiceResponse<IEnumerable<OrderDto>>>> CancelOrder(int orderId)
	{
		var userOrder = await _unitOfWork.Order.GetFirstOrDefault((o => o.Id == orderId));

		userOrder.OrderStatus = -1;
		await _unitOfWork.SaveAsync();

		var orderDtos = new List<OrderDto>();
		var result = (await _unitOfWork.Order.GetAll())
					.OrderByDescending(o => o.OrderDate);

		foreach(var order in result)
		{
			orderDtos.Add(new OrderDto
			{
				User = await _unitOfWork.Auth.GetFirstOrDefault((u => u.Id == order.UserId), includeProperties: "Address"),
				Order = order
			});
		}
		return Ok(new ServiceResponse<IEnumerable<OrderDto>>
		{
			Data = orderDtos
		});
	}

	[HttpGet("admin/get-order-model/{orderId}")]
	public async Task<ActionResult<ServiceResponse<Order>>> GetOrderModel(int orderId)
	{
		var order = await _unitOfWork.Order
			.GetFirstOrDefault((o => o.Id == orderId));

		return Ok(new ServiceResponse<Order> { Data = order });
	}
}