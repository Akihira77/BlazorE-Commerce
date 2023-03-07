using BlazorEcommerce.Server.Services.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;

namespace BlazorEcommerce.Server.Controllers;
[Route("api/v1/[controller]")]
[ApiController]
public class CartController : ControllerBase
{
	private readonly IUnitOfWork _unitOfWork;

	public CartController(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
	}

	[HttpPost("products")]
	public async Task<ActionResult<ServiceResponse<IEnumerable<CartProductDto>>>> GetCartProducts(IEnumerable<CartItem> cartItems)
	{
		var response = new ServiceResponse<IEnumerable<CartProductDto>>()
		{
			Data = await _unitOfWork.Cart.GetCartProducts(cartItems),
		};

		return Ok(response);
	}
}
