using BlazorEcommerce.Server.Services.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
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
			Data = await _unitOfWork.Category
					.GetAll((c => !c.Deleted && c.Visible)),
			Message = "Category list",
		};

		return Ok(response);
	}

	[HttpGet("admin"), Authorize(Roles = "Admin")]
	public async Task<ActionResult<ServiceResponse<IEnumerable<Category>>>> AdminGetAllCategories()
	{
		var response = new ServiceResponse<IEnumerable<Category>>()
		{
			Data = await _unitOfWork.Category
			.GetAll((c => !c.Deleted)),
			Message = "Admin - Category list",
		};

		return Ok(response);
	}

	[HttpPost("admin"), Authorize(Roles = "Admin")]
	public async Task<ActionResult<ServiceResponse<IEnumerable<Category>>>> AddCategory(Category category)
	{
		category.Editing = category.IsNew = false;
		await _unitOfWork.Category.Add(category);
		await _unitOfWork.Save();
		var response = new ServiceResponse<IEnumerable<Category>>
		{
			Data = await _unitOfWork.Category
			.GetAll((c => !c.Deleted)),
			Message = $"Adding Category {category.Name} is success"
		};

		return Ok(response);
	}

	[HttpDelete("admin/{id}"), Authorize(Roles = "Admin")]
	public async Task<ActionResult<ServiceResponse<IEnumerable<Category>>>> DeleteCategory(int id)
	{
		var category = await _unitOfWork.Category.GetFirstOrDefault((c => c.Id == id));
		var response = new ServiceResponse<IEnumerable<Category>>();

		if(category != null)
		{
			category.Deleted = true;
			response.Message = "Deleting category is success";
			//_unitOfWork.Category.Remove(category);
			await _unitOfWork.Save();
		} else
		{
			response.Success = false;
			response.Message = "Category is not found";
		}
		response.Data = await _unitOfWork.Category
			.GetAll((c => !c.Deleted));
		return Ok(response);
	}

	[HttpPut("admin"), Authorize(Roles = "Admin")]
	public async Task<ActionResult<ServiceResponse<IEnumerable<Category>>>> UpdateCategory(Category category)
	{
		var dbCategory = await _unitOfWork.Category.GetFirstOrDefault((c => c.Id == category.Id), track: false);

		var response = new ServiceResponse<IEnumerable<Category>>();
		if(category != null)
		{
			response.Message = "Updating category is success";
			category.Editing = false;
			_unitOfWork.Category.Update(category);
			await _unitOfWork.Save();
		} else
		{
			response.Success = false;
			response.Message = "Category is not found";
		}
		response.Data = await _unitOfWork.Category
			.GetAll((c => !c.Deleted));
		return Ok(response);
	}
}
