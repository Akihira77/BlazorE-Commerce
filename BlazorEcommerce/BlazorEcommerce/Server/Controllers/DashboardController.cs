using BlazorEcommerce.Server.Services.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlazorEcommerce.Server.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class DashboardController : ControllerBase
{
	private readonly IUnitOfWork _unitOfWork;

	public DashboardController(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
	}

	[HttpGet]
	public async Task<ActionResult<ServiceResponse<DashboardDto>>> Main()
	{
		var dashboardDto = new DashboardDto
		{
			Products = await _unitOfWork.Product.GetAll(),
			Categories = await _unitOfWork.Category.GetAll(),
			Users = await _unitOfWork.Auth.GetAll(),
			Orders = await _unitOfWork.Order.GetAll(),
			ProductTypes = await _unitOfWork.ProductType.GetAll()
		};

		var orderList = (await _unitOfWork.Order.GetAll((o => o.OrderStatus != -1)))
					.OrderByDescending(o => o.OrderDate);

		var omzetList = new IncomeDto();
		decimal[] omzetPerMonth = new decimal[13];
		foreach(var order in orderList)
		{
			var date = order.OrderDate.ToString("MM");
			omzetPerMonth[int.Parse(date)] += order.TotalPrice;
		}

		for(var i = 1; i <= 12; i++)
		{
			omzetList.Income.Add(omzetPerMonth[i]);
		}

		dashboardDto.IncomeDto = omzetList;

		return Ok(new ServiceResponse<DashboardDto>
		{
			Data = dashboardDto
		});
	}
}