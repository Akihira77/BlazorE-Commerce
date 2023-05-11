using Stripe.Checkout;

namespace BlazorEcommerce.Server.Features.Payments;

public interface IPaymentRepository
{
    Task<Session> CreateCheckoutSession();

    Task<ServiceResponse<int>> FulfillOrder(HttpRequest request);
}