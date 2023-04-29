using BlazorEcommerce.Server.Services.Repositories.IRepositories;
using BlazorEcommerce.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlazorEcommerce.Server.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly ILogger<ProductController> _logger;

	public ProductController(IUnitOfWork unitOfWork, ILogger<ProductController> logger)
	{
		_unitOfWork = unitOfWork;
		_logger = logger;
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
		await _unitOfWork.Log.Add(new Logs
		{
			LogEvent = $"User '{_unitOfWork.Auth.GetUserEmail()}' Adding new 'Product : {product.Title}'",
		});

		await _unitOfWork.SaveAsync();

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
			await _unitOfWork.Log.Add(new Logs
			{
				LogEvent = $"User '{_unitOfWork.Auth.GetUserEmail()}' Updating a 'Product : {product.Title}'",
			});
			await _unitOfWork.SaveAsync();
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
			await _unitOfWork.Log.Add(new Logs
			{
				LogEvent = $"User '{_unitOfWork.Auth.GetUserEmail()}' Deleting a 'Product : {product.Title}'",
			});
			await _unitOfWork.SaveAsync();
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

	[HttpGet("Get-product-ratings/{id}")]
	public async Task<ActionResult<ServiceResponse<IEnumerable<ProductRatingsDto>>>> GetProductRatings(int id)
	{
		var response = new ServiceResponse<List<ProductRatingsDto>>()
		{
			Success = false,
			Message = $"Sorry but this product does not exist.",
		};

		var result = await _unitOfWork.Product
				.GetProductRatings(id);

		if(result != null)
		{
			response.Success = true;

			var pr = new List<ProductRatingsDto>();

			foreach(var productRatings in result)
			{
				pr.Add(new ProductRatingsDto
				{
					Rate = productRatings.Rate,
					TextReviews = productRatings.Reviews,
					UserName = productRatings.User.Email,
					CreatedOn = productRatings.CreatedOn
				});
			}
			response.Data = pr;
			response.Message = $"Get product {id} with ratings is success";
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

	[HttpPost("submit-reviews")]
	public async Task<ActionResult<ServiceResponse<bool>>> SubmitReviews(ProductRatings productRatings)
	{
		var userId = _unitOfWork.Auth.GetUserId();
		var productRatingsDb = await _unitOfWork.ProductRatings
								.GetFirstOrDefault((pr => pr.UserId == userId
													&& pr.ProductId == productRatings.ProductId));

		if(productRatingsDb == null)
		{
			productRatings.UserId = userId;
			productRatings.User = await _unitOfWork.Auth.GetFirstOrDefault((u => u.Id == userId));

			try
			{
				await _unitOfWork.ProductRatings.Add(productRatings);
				await _unitOfWork.SaveAsync();

				return new ServiceResponse<bool>
				{
					Success = true,
					Message = "Reviews Added"
				};
			} catch(Exception ex)
			{
				_logger.LogError(ex.Message);

				return new ServiceResponse<bool>
				{
					Success = false,
					Message = "Got Error while saving reviews"
				};
			}
		} else
		{
			return new ServiceResponse<bool>
			{
				Success = false,
				Message = "Sorry, You're already give reviews for this product"
			};
		} 
	}
}