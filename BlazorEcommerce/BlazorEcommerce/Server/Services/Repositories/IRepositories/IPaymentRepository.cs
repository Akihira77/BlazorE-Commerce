using Stripe.Checkout;

namespace BlazorEcommerce.Server.Services.Repositories.IRepositories;

public interface IPaymentRepository
{
	Task<Session> CreateCheckoutSession();

	Task<ServiceResponse<int>> FulfillOrder(HttpRequest request);
}