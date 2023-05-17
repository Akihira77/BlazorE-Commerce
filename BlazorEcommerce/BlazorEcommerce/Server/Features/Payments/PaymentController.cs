using BlazorEcommerce.Server.Features.Base;
using BlazorEcommerce.Server.Features.Emails;
using BlazorEcommerce.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;

namespace BlazorEcommerce.Server.Features.Payments;

[Route("api/v1/[controller]")]
[ApiController]
public class PaymentController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailSender _emailSender;

	public PaymentController(IUnitOfWork unitOfWork,
        IEmailSender emailSender)
    {
        _unitOfWork = unitOfWork;
        _emailSender = emailSender;
    }

    [HttpPost("checkout"), Authorize]
    public async Task<string> CreateCheckoutSession()
    {
        //var session = await _unitOfWork.Payment.CreateCheckoutSession();
        var cartItems = await _unitOfWork.Cart
                    .GetAll(c => c.UserId == _unitOfWork.Auth.GetUserId());
        var products = await _unitOfWork.Cart.GetCartProducts(cartItems);

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
                    }
                },
                Quantity = product.Quantity,
            });
        }

        var orderHeader = new OrderHeader
        {
            IsPaid = false,
            UserId = _unitOfWork.Auth.GetUserId(),
        };

        //await _unitOfWork.Save();

        var options = new SessionCreateOptions
        {
            CustomerEmail = _unitOfWork.Auth.GetUserEmail(),
            PaymentMethodTypes = new List<string>
                {
                    "card"
                },
            LineItems = lineItems,
            Mode = "payment",
            //SuccessUrl = $"https://localhost:7155/order-success",
            //CancelUrl = "https://localhost:7155/cart"
            SuccessUrl = "https://blazoreco.azurewebsites.net/order-success",
            CancelUrl = "https://blazoreco.azurewebsites.net/cart"
        };

        var service = new SessionService();
        Session session = service.Create(options);

        orderHeader.SessionId = session.Id;

        await _unitOfWork.OrderHeader.Add(orderHeader);

        await _unitOfWork.Log.Add(new Logs
        {
            LogEvent = $"User '{_unitOfWork.Auth.GetUserEmail()}' Click the Checkout",
        });

        await _unitOfWork.SaveAsync();

        return session.Url;
    }

    [HttpPost]
    public async Task<ActionResult<ServiceResponse<bool>>> FulfillOrder()
    {
        var request = await _unitOfWork.Payment.FulfillOrder(Request);
        var response = new ServiceResponse<bool>();
        if (request.Success)
        {
            if (request.Data > 0)
            {
                var user = await _unitOfWork.Auth.GetFirstOrDefault(u => u.Id == request.Data);
                var cartItems = await _unitOfWork.Cart.GetAll(c => c.UserId == user.Id);
                var cartProducts = await _unitOfWork.Cart.GetCartProducts(cartItems);
                var orderHeader = await _unitOfWork.OrderHeader
                            .GetFirstOrDefault(oh => oh.SessionId.Equals(request.Message));

                foreach (var product in cartProducts)
                {
                    var dbProduct = await _unitOfWork.Product.GetFirstOrDefault(p => p.Id == product.ProductId);
                    //if(dbProduct != null)
                    //{
                    //    dbProduct.Stock -= product.Quantity;
                    //}
                }

                var order = _unitOfWork.Order.PlaceOrder(cartProducts, user.Id);

                await _unitOfWork.Order.Add(order);

                _unitOfWork.Cart.RemoveRange(cartItems);

                _unitOfWork.Save();
                orderHeader.OrderId = order.Id;
                orderHeader.IsPaid = true;

                await _unitOfWork.Log.Add(new Logs
                {
                    LogEvent = $"User '{user.Email}' success fulfill the order",
                });

                await _unitOfWork.SaveAsync();

                response.Message = "Order has been fulfilled";

                var message = new Message(
                    new string[] { user.Email },
                        "Checkout",
                        "A Checkout has been completed");
                await _emailSender.SendEmailAsync(message);
            }
            return Ok(response);
        }
        else
        {
            response.Message = request.Message;
            response.Success = false;
            return BadRequest(response);
        }
    }
}