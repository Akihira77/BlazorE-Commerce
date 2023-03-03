using BlazorEcommerce.Server.Services.Repositories.IRepositories;
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

		if (result != null)
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
        var result = await _unitOfWork.Product
				.GetAll((p => p.Category.Url.ToLower().Equals(categoryUrl)) 
						,includeProperties: "Variants");

        var response = new ServiceResponse<IEnumerable<Product>>()
        {
            Data = result,
            Message = "Product List"
        };
        return Ok(response);
    }
}
