using BlazorEcommerce.Server.Services.Repositories.IRepositories;
using BlazorEcommerce.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace BlazorEcommerce.Server.Controllers;
[Route("api/v1/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMemoryCache _memoryCache;

	public ProductController(IUnitOfWork unitOfWork, IMemoryCache memoryCache)
	{
		_unitOfWork = unitOfWork;
		_memoryCache = memoryCache;
	}

	[HttpGet("Get-all-products")]
	public async Task<ActionResult<ServiceResponse<IEnumerable<Product>>>> GetAllProducts()
	{
		var result = await _unitOfWork.Product.GetAll(includeProperties: "Variants");
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
		var productsByCategory = _memoryCache.TryGetValue(categoryUrl, out result);

		var response = new ServiceResponse<IEnumerable<Product>>()
		{
			Message = $"Product list by category {categoryUrl}"
		};

		if(productsByCategory)
		{
			response.Message = $"Product list by category {categoryUrl} from cache";
		} else
		{
			result = await _unitOfWork.Product
					.GetAll((p => p.Category.Url.ToLower().Equals(categoryUrl))
							, includeProperties: "Variants");

			var cacheEntryOptions = new MemoryCacheEntryOptions()
			.SetAbsoluteExpiration(TimeSpan.FromDays(1));

			_memoryCache.Set(categoryUrl, result, cacheEntryOptions);
		}

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

		var featuredProductCache = _memoryCache.TryGetValue(CacheKeys.FeaturedCacheKey, out result);

		if(featuredProductCache)
		{
			response.Message = "Product featured cache list";
		} else
		{
			result = await _unitOfWork.Product.GetAll((p => p.Featured), includeProperties: "Variants");

			var cacheEntryOptions = new MemoryCacheEntryOptions()
						.SetAbsoluteExpiration(TimeSpan.FromDays(1));

			_memoryCache.Set(CacheKeys.FeaturedCacheKey, result, cacheEntryOptions);
		}
		response.Data = result;
		return Ok(response);
	}
}
