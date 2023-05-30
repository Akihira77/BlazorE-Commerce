namespace BlazorEcommerce.Server.Features.Emails;

public interface IEmailSender
{
    Task<bool> SendEmailAsync(Message message);
}