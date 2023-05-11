using BlazorEcommerce.Server.Features.Base;
using BlazorEcommerce.Shared.Dto.CartDTO;
using BlazorEcommerce.Shared.Dto.OrderDTO;

namespace BlazorEcommerce.Server.Features.Orders;

public interface IOrderRepository : IRepository<Order>
{
    Order PlaceOrder(IEnumerable<CartProductDto> cartProducts, int? userId = null);

    Task<IEnumerable<OrderOverviewDto>> GetOrders();

    Task<OrderDetailsDto> GetOrderDetails(int orderId);

    Task<OrderDetailsDto> AdminGetOrderDetails(int orderId);
}