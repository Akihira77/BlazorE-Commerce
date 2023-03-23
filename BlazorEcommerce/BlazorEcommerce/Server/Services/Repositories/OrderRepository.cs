using BlazorEcommerce.Server.Data;
using BlazorEcommerce.Server.Services.Repositories.IRepositories;
using BlazorEcommerce.Shared.Models;

namespace BlazorEcommerce.Server.Services.Repositories;

public class OrderRepository : Repository<Order>, IOrderRepository
{
	private readonly AppDbContext _db;
	private readonly IAuthRepository _authRepository;

	public OrderRepository(AppDbContext db, IAuthRepository authRepository) : base(db)
	{
		_db = db;
		_authRepository = authRepository;
	}

	public async Task<OrderDetailsDto> GetOrderDetails(int orderId)
	{
		var orderDetailsDto = new OrderDetailsDto();

		var order = await _db.Orders
			.Include(o => o.OrderItems)
			.ThenInclude(oi => oi.Product)
				.ThenInclude(p => p.Images)
			.Include(o => o.OrderItems)
			.ThenInclude(oi => oi.ProductType)
			.Where(o => o.UserId == _authRepository.GetUserId() && o.Id == orderId)
			.OrderByDescending(o => o.OrderDate)
			.FirstOrDefaultAsync();

		if (order == null)
		{
			return null;
		}

		orderDetailsDto.OrderDate = order.OrderDate;
		orderDetailsDto.TotalPrice = order.TotalPrice;
		var temp = new List<OrderDetailsProductDto>();

		foreach(var item in order.OrderItems)
		{
			temp.Add(new OrderDetailsProductDto
			{
				ProductId = item.ProductId,
				ProductType = item.ProductType.Name,
				ImageUrl = !string.IsNullOrEmpty(item.Product.ImageUrl)? item.Product.ImageUrl : item.Product.Images[0].Data,
				Quantity = item.Quantity,
				Title = item.Product.Title,
				TotalPrice = item.TotalPrice
			});
		}

		orderDetailsDto.Products = temp.AsEnumerable();

		return orderDetailsDto;
	}

	public async Task<IEnumerable<OrderOverviewDto>> GetOrders()
	{
		var orders = await _db.Orders
				.Include(o => o.OrderItems)
					.ThenInclude(oi => oi.Product)
					.ThenInclude(p => p.Images)
				.Where(o => o.UserId == _authRepository.GetUserId())
				.OrderByDescending(o => o.OrderDate)
				.ToListAsync();


		var orderOverview = new List<OrderOverviewDto>();
		foreach (var order in orders)
		{
			orderOverview.Add(new OrderOverviewDto
			{
				Id = order.Id,
				OrderDate = order.OrderDate,
				TotalPrice = order.TotalPrice,
				Product = order.OrderItems.Count() > 1?
					$"{order.OrderItems.First().Product.Title} and " + 
					$"{order.OrderItems.Count() - 1} more ..." 
						: 
					order.OrderItems.First().Product.Title,
				ProductImageUrl = !string.IsNullOrEmpty(order.OrderItems.First().Product.ImageUrl) ?
									order.OrderItems.First().Product.ImageUrl
									:
									order.OrderItems.First().Product.Images[0].Data
			});
		}

		return orderOverview.AsEnumerable();
	}

	public Order PlaceOrder(IEnumerable<CartProductDto> cartProducts, int? userId = null)
	{
		decimal totalPrice = 0;

		var orderItems = new List<OrderItem>();
		foreach(var product in cartProducts)
		{
			totalPrice += (product.Quantity * product.Price);
			orderItems.Add(new OrderItem
			{
				ProductId = product.ProductId,
				ProductTypeId = product.ProductTypeId,
				Quantity = product.Quantity,
				TotalPrice = product.Price * product.Quantity,
			});
		}

		var order = new Order
		{
			UserId = ((int)(userId == 0 || userId == null? _authRepository.GetUserId() : userId)),
			OrderDate = DateTime.Now,
			TotalPrice = totalPrice,
			OrderItems = orderItems
		};

		return order;
	}
}
