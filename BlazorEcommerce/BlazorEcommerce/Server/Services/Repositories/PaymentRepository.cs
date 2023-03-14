using BlazorEcommerce.Server.Services.Repositories.IRepositories;
using Newtonsoft.Json;
using Stripe;
using Stripe.Checkout;
using System.Diagnostics.Tracing;

namespace BlazorEcommerce.Server.Services.Repositories;

public class PaymentRepository : IPaymentRepository
{
    private readonly ICartRepository _cartRepository;
    private readonly IAuthRepository _authRepository;
    private readonly IOrderRepository _orderRepository;

	// just for 90 day
	// need to re-authenticate use strip cli to get another secret strings
	const string secret = "whsec_d8ad074fdaa62c2a85136958b93cf6657fa863fd8e0146af3d8d542ab3a7f2db";

	public PaymentRepository(
        ICartRepository cartRepository
        , IAuthRepository authRepository
        , IOrderRepository orderRepository)
    {
        StripeConfiguration.ApiKey = "sk_test_51MjvH4Fa0qfePvUBN1lvujqHlTFa3zRr0MqWAcRxXOdRxNjUsj6XJQNvcyhSUA0YcVxtFXE7guZtK3kukBaJbtih00VuYbLkl6";

        _cartRepository = cartRepository;
        _authRepository = authRepository;
        _orderRepository = orderRepository;
    }
    public async Task<Session> CreateCheckoutSession()
    {
        var cartItems = await _cartRepository.GetAll(c => c.UserId == _authRepository.GetUserId());
        var products = await _cartRepository.GetCartProducts(cartItems);

        var lineItems = new List<SessionLineItemOptions>();
        foreach (var product in products)
        {
            lineItems.Add(new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmountDecimal = product.Price * 100,
                    Currency = "usd",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = product.Title,
                        Images = new List<string> { product.ImageUrl }
                    }
                },
                Quantity = product.Quantity,
            });
        }

        var options = new SessionCreateOptions
        {
            CustomerEmail = _authRepository.GetUserEmail(),
            ShippingAddressCollection = new SessionShippingAddressCollectionOptions
            {
                AllowedCountries = new List<string> { "ID" }
            },
            PaymentMethodTypes = new List<string>
                {
                    "card"
                },
            LineItems = lineItems,
            Mode = "payment",
            SuccessUrl = "https://localhost:7155/order-success",
            CancelUrl = "https://localhost:7155/cart"
        };

        var service = new SessionService();
        Session session = service.Create(options);
        return session;
    }

	public async Task<ServiceResponse<int>> FulfillOrder(HttpRequest request)
	{
        var json = await new StreamReader(request.Body).ReadToEndAsync();
        try
        {
            var stripeEvent = EventUtility
                .ConstructEvent(
                    json,
                    request.Headers["Stripe-Signature"],
                    secret
                );

            var user = new User();
			//stripeEvent.Type == Events.CheckoutSessionCompleted
			//	|| stripeEvent.Type == Events.PaymentIntentSucceeded
			//	|| stripeEvent.Type == Events.PaymentIntentCreated
			if (stripeEvent.Type == Events.CheckoutSessionCompleted)
			{
				var session = stripeEvent.Data.Object as Session;
                user = await _authRepository.GetFirstOrDefault((u => u.Email.Equals(session.CustomerEmail)));
            }
			return new ServiceResponse<int> { Data = user.Id, Message = "Fulfill order successful"}; 

        } catch(StripeException ex)
        {
            return new ServiceResponse<int> { Success = false, Message = ex.Message };
        }
	}
}
