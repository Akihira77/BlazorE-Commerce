using BlazorEcommerce.Server.Services.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlazorEcommerce.Server.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize(Roles = "Admin")]
public class ProductTypeController : ControllerBase
{
	private readonly IUnitOfWork _unitOfWork;

	public ProductTypeController(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
	}

	[HttpGet("get-all-producttype")]
	public async Task<ActionResult<ServiceResponse<List<ProductType>>>> GetAllProductTypes()
	{
		var response = new ServiceResponse<List<ProductType>>
		{
			Data = (await _unitOfWork.ProductType.GetAll()).ToList(),
			Message = "Product type list"
		};

		return Ok(response);
	}

	[HttpPost("add-producttype")]
	public async Task<ActionResult<ServiceResponse<List<ProductType>>>> AddProductType(ProductType productType)
	{
		productType.Editing = productType.IsNew = false;
		await _unitOfWork.ProductType.Add(productType);
		await _unitOfWork.Log.Add(new Logs
		{
			LogEvent = $"User '{_unitOfWork.Auth.GetUserEmail()}' Adding new 'ProductType : {productType.Name}'",
		});
		await _unitOfWork.SaveAsync();

		var response = new ServiceResponse<List<ProductType>>
		{
			Data = (await _unitOfWork.ProductType.GetAll()).ToList(),
			Message = "Adding Product Type is success"
		};

		return Ok(response);
	}

	[HttpPut("update-producttype")]
	public async Task<ActionResult<ServiceResponse<List<ProductType>>>> UpdateProductType(ProductType productType)
	{
		var result = await _unitOfWork.ProductType.GetFirstOrDefault((pt => pt.Id == productType.Id), track: false);

		var response = new ServiceResponse<List<ProductType>>();

		if(result == null)
		{
			response.Success = false;
			response.Message = "Product type is not found";
		} else
		{
			productType.Editing = false;
			_unitOfWork.ProductType.Update(productType);
			await _unitOfWork.Log.Add(new Logs
			{
				LogEvent = $"User '{_unitOfWork.Auth.GetUserEmail()}' Updating a 'ProductType : {productType.Name}'",
			});
			await _unitOfWork.SaveAsync();
			response.Data = (await _unitOfWork.ProductType.GetAll()).ToList();
			response.Message = "Updating product type is success";
		}

		return Ok(response);
	}

	[HttpDelete("delete-producttype/{id}")]
	public async Task<ActionResult<ServiceResponse<IEnumerable<ProductType>>>> DeleteProductType(int id)
	{
		var productType = await _unitOfWork.ProductType.GetFirstOrDefault((pt => pt.Id == id));

		var response = new ServiceResponse<IEnumerable<ProductType>>();
		if(productType != null)
		{
			_unitOfWork.ProductType.Remove(productType);
			await _unitOfWork.Log.Add(new Logs
			{
				LogEvent = $"User '{_unitOfWork.Auth.GetUserEmail()}' Deleting a 'ProductType : {productType.Name}'",
			});
			await _unitOfWork.SaveAsync();

			response.Message = "Deleting Product Type is success";
		} else
		{
			response.Message = "Product Type is not found";
		}

		response.Data = await _unitOfWork.ProductType.GetAll();
		return Ok(response);
	}

	[HttpGet("producttype-category/{categoryId}")]
	public async Task<ActionResult<ServiceResponse<IEnumerable<ProductType>>>> GetProductTypeByCategory(int categoryId)
	{
		var response = new ServiceResponse<IEnumerable<ProductType>>
		{
			Data = (await _unitOfWork.ProductType
					.GetAll((pt => pt.CategoryId == categoryId)))
					.AsEnumerable(),
		};

		return Ok(response);
	}
}