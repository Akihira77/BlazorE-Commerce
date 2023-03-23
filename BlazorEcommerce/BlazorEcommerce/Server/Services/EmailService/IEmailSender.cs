namespace BlazorEcommerce.Server.Services.EmailService;

public interface IEmailSender
{
	Task SendEmailAsync(Message message);
}
