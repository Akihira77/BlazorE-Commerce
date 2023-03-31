using BlazorEcommerce.Server.Services.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlazorEcommerce.Server.Controllers;
[Route("api/v1/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
	private readonly IUnitOfWork _unitOfWork;

	public ProductController(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
	}

	[HttpGet("admin"), Authorize(Roles = "Admin")]
	public async Task<ActionResult<ServiceResponse<IEnumerable<Product>>>> AdminGetAllProducts()
	{
		IEnumerable<Product> result;

		var response = new ServiceResponse<IEnumerable<Product>>()
		{
			Message = "Admin Product List"
		};

		result = await _unitOfWork.Product.GetAdminProducts();

		response.Data = result;
		return Ok(response);
	}

	[HttpPost("add-product"), Authorize(Roles = "Admin")]
	public async Task<ActionResult<ServiceResponse<Product>>> AddProduct(Product product)
	{
		foreach(var variant in product.Variants)
		{
			variant.ProductType = null;
		}
		await _unitOfWork.Product.Add(product);
		await _unitOfWork.Save();

		var response = new ServiceResponse<Product>
		{
			Data = product,
			Message = "Adding new product is success"
		};
		return Ok(response);
	}

	[HttpPut("update-product"), Authorize(Roles = "Admin")]
	public async Task<ActionResult<ServiceResponse<Product>>> UpdateProduct(Product product)
	{
		var dbProduct = await _unitOfWork.Product
			.GetFirstOrDefault((p => p.Id == product.Id), track: false, includeProperties: "Images");

		var response = new ServiceResponse<Product>
		{
			Message = "Updating product is success"
		};
		if(dbProduct != null)
		{
			foreach(var variant in product.Variants)
			{
				var dbVariant = await _unitOfWork.ProductVariant
					.GetFirstOrDefault((pv => pv.ProductId == variant.ProductId
									&& pv.ProductTypeId == variant.ProductTypeId), track: false);

				if(dbVariant == null)
				{
					variant.ProductType = null;
					await _unitOfWork.ProductVariant.Add(variant);
				} else
				{
					//dbVariant.ProductTypeId = variant.ProductId;
					//dbVariant.Price = variant.Price;
					//dbVariant.OriginalPrice = variant.OriginalPrice;
					//dbVariant.Visible = variant.Visible;
					//dbVariant.Deleted = variant.Deleted;
					_unitOfWork.ProductVariant.Update(variant);
				}
			}

			_unitOfWork.Images.RemoveRange(dbProduct.Images);

			_unitOfWork.Product.Update(product);
			await _unitOfWork.Save();
			response.Data = product;
		} else
		{
			response.Success = false;
			response.Message = "Product is not found";
		}
		return Ok(response);
	}

	[HttpDelete("delete-product/{id}"), Authorize(Roles = "Admin")]
	public async Task<ActionResult<ServiceResponse<bool>>> DeleteProduct(int id)
	{
		var product = await _unitOfWork.Product
			.GetFirstOrDefault((p => p.Id == id));
		var response = new ServiceResponse<bool>
		{
			Message = "Deleting productis success",
			Data = true
		};

		if(product == null)
		{
			response.Success = response.Data = false;
			response.Message = "Product is not found";
		} else
		{
			//_unitOfWork.Product.Remove(product);
			product.Deleted = true;
			await _unitOfWork.Save();
		}
		return Ok(response);
	}

	[HttpGet("Get-all-products")]
	public async Task<ActionResult<ServiceResponse<IEnumerable<Product>>>> GetAllProducts()
	{
		var result = await _unitOfWork.Product
				.GetProducts();
		var response = new ServiceResponse<IEnumerable<Product>>()
		{
			Data = result,
			Message = "Product List"
		};
		return Ok(response);
	}

	[HttpGet("Get-product/{id}")]
	public async Task<ActionResult<ServiceResponse<Product>>> GetProduct(int id)
	{
		var response = new ServiceResponse<Product>()
		{
			Success = false,
			Message = $"Sorry but this product does not exist.",
		};

		var result = await _unitOfWork.Product
				.GetProduct(id);

		if(result != null)
		{
			response.Success = true;
			response.Data = result;
			response.Message = $"Get product {id} is success";
		}

		return Ok(response);
	}

	[HttpGet("Get-products-from/{categoryUrl}")]
	public async Task<ActionResult<ServiceResponse<IEnumerable<Product>>>> GetProductsBy(string categoryUrl)
	{
		IEnumerable<Product> result;

		var response = new ServiceResponse<IEnumerable<Product>>()
		{
			Message = $"Product list by category {categoryUrl}"
		};

		result = await _unitOfWork.Product
					.GetProductsByCategory(categoryUrl);

		response.Data = result;
		return Ok(response);
	}

	[HttpGet("search-products/{searchText}/{page}")]
	public async Task<ActionResult<ServiceResponse<ProductSearchDto>>> SearchProducts(string searchText, int page = 1)
	{
		var result = await _unitOfWork.Product.SearchProducts(searchText, page);

		var response = new ServiceResponse<ProductSearchDto>()
		{
			Data = result,
			Message = "Product search List"
		};
		return Ok(response);
	}

	[HttpGet("search-suggestion/{searchText}")]
	public async Task<ActionResult<ServiceResponse<IEnumerable<string>>>> SearchProductsSuggestion(string searchText)
	{
		var result = _unitOfWork.Product.GetProductSearchSuggestion(searchText);

		var response = new ServiceResponse<IEnumerable<string>>()
		{
			Data = result,
			Message = "Product suggestion List"
		};
		return Ok(response);
	}

	[HttpGet("featured")]
	public async Task<ActionResult<ServiceResponse<IEnumerable<Product>>>> GetFeaturedProducts()
	{
		IEnumerable<Product> result;

		var response = new ServiceResponse<IEnumerable<Product>>()
		{
			Message = "Product featured List"
		};

		result = await _unitOfWork.Product.GetFeaturedProducts();

		response.Data = result;
		return Ok(response);
	}
}
