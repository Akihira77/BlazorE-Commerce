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
			Message = "Cart products from the receive cartItems"
		};

		return Ok(response);
	}

	[HttpPost]
	public async Task<ActionResult<ServiceResponse<IEnumerable<CartProductDto>>>> StoreCartItems(IEnumerable<CartItem> cartItems)
	{
		var result = _unitOfWork.Cart.StoreCartItems(cartItems);

		await _unitOfWork.Cart.AddRange(result);
		await _unitOfWork.SaveAsync();

		var response = GetDbCartProducts();

		return Ok(response);
	}

	[HttpGet("count")]
	public async Task<ActionResult<ServiceResponse<int>>> GetCartItemsCount()
	{
		var response = new ServiceResponse<int>
		{
			Data = await _unitOfWork.Cart.GetCartItemsCount(),
			Message = "Sum of quantity each products"
		};

		return Ok(response);
	}

	[HttpGet]
	public async Task<ActionResult<ServiceResponse<IEnumerable<CartProductDto>>>> GetDbCartProducts()
	{
		var result = await _unitOfWork.Cart.GetAll((c => c.UserId == _unitOfWork.Auth.GetUserId()));

		var response = new ServiceResponse<IEnumerable<CartProductDto>>
		{
			Data = await _unitOfWork.Cart.GetCartProducts(result),
			Message = "Cart products from db list"
		};

		return Ok(response);
	}

	[HttpPost("add")]
	public async Task<ActionResult<ServiceResponse<bool>>> AddToCart(CartItem cartItem)
	{
		var response = new ServiceResponse<bool> { Success = false };
		cartItem.UserId = _unitOfWork.Auth.GetUserId();

		var sameItem = await _unitOfWork.Cart
			.GetFirstOrDefault((c => c.UserId == cartItem.UserId
			&& c.ProductId == cartItem.ProductId
			&& c.ProductTypeId == cartItem.ProductTypeId));

		if(sameItem != null)
		{
			sameItem.Quantity = cartItem.Quantity;
			response.Message = "update quantity success";
			response.Success = true;
		} else
		{
			await _unitOfWork.Cart.Add(cartItem);
			response.Message = "added new cartItem to db success";
			response.Success = true;
		}
		response.Data = response.Success;
		await _unitOfWork.SaveAsync();
		return Ok(response);
	}

	[HttpDelete("remove-cart-item/{productId}&&{productTypeId}")]
	public async Task<ActionResult<ServiceResponse<bool>>> RemoveCartItem(int productId, int productTypeId)
	{
		var response = new ServiceResponse<bool>();

		var cartItem = await _unitOfWork.Cart
			.GetFirstOrDefault((c => c.UserId == _unitOfWork.Auth.GetUserId()
			&& c.ProductId == productId
			&& c.ProductTypeId == productTypeId));

		if(cartItem != null)
		{
			_unitOfWork.Cart.Remove(cartItem);
			await _unitOfWork.SaveAsync();
			response.Message = "Deleting cart item from db success";
		} else
		{
			response.Message = "Error!. Cart item is not found";
			response.Success = false;
		}
		response.Data = response.Success;
		return Ok(response);
	}
}
