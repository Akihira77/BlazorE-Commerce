namespace BlazorEcommerce.Server.Features.Emails;

public interface IEmailSender
{
    Task SendEmailAsync(Message message);
}