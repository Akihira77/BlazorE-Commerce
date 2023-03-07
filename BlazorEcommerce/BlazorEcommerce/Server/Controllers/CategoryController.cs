using BlazorEcommerce.Server.Services.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;

namespace BlazorEcommerce.Server.Controllers;
[Route("api/v1/[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
	private readonly IUnitOfWork _unitOfWork;

	public CategoryController(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
	}

	[HttpGet("Get-all-categories")]
	public async Task<ActionResult<ServiceResponse<IEnumerable<Category>>>> GetCategories()
	{
		var response = new ServiceResponse<IEnumerable<Category>>()
		{
			Data = await _unitOfWork.Category.GetAll(),
			Message = "Category list",
		};

		return Ok(response);
	}
}
