using BlazorEcommerce.Server.Services.Repositories.IRepositories;
using BlazorEcommerce.Shared.Models;
using MailKit.Net.Smtp;
using MimeKit;
using System.Text;

namespace BlazorEcommerce.Server.Services.EmailService;

public class EmailSender : IEmailSender
{
	private readonly EmailConfiguration _emailConfiguration;
	private readonly IUnitOfWork _unitOfWork;

	public EmailSender(EmailConfiguration emailConfiguration, IUnitOfWork unitOfWork)
    {
		_emailConfiguration = emailConfiguration;
		_unitOfWork = unitOfWork;
	}
    public async Task SendEmailAsync(Message message)
	{
		var emailMessage = await CreateEmailMessage(message);

		await SendAsync(emailMessage);
	}

	private async Task<MimeMessage> CreateEmailMessage(Message message)
	{
		var emailMessage = new MimeMessage();
		emailMessage.From.Add(new MailboxAddress("email", _emailConfiguration.From));
		emailMessage.To.AddRange(message.To);

		if(message.Subject.ToLower().Equals("otp"))
		{
			emailMessage.Subject = "One Time Password";
			emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
			{
				Text = $"<p>This is your OTP Code <span style='font-weight: 600'>{message.Content}</span></p>" +
					$"<br /> <p>This code can be use just for <span style='font-weight: 600'>3 minutes</span></p>"
			};
		} else if (message.Subject.ToLower().Equals("order"))
		{
			emailMessage.Subject = "Blazor ECommerce Order Notification";

			var order = await _unitOfWork.Order.GetOrderDetails(int.Parse(message.Content));
			var file = File.ReadAllText("F:\\File_Mahasiswa\\Programming\\dotnet\\BlazorE-Commerce\\BlazorEcommerce\\BlazorEcommerce\\Server\\StaticFiles\\Html\\InvoiceTop.html");
			var htmlOrderItems = "<tr class=\"item\">\r\n    <td>{0}</td>\r\n\r\n    <td>{1}</td>\r\n</tr>\r\n\r\n";

			var sb = new StringBuilder();
			foreach(var item in order.Products)
			{
				sb.AppendFormat(htmlOrderItems, item.Title, item.TotalPrice.ToString("c"));
			}

			file = file.Replace("{{0}}", message.Content);
			file = file.Replace("{{1}}", order.OrderDate.ToString("f"));
			file = file.Replace("{{2}}", order.TotalPrice.ToString("c"));
			file = file.Replace("{{3}}", sb.ToString());
			file = file.Replace("{{4}}", order.TotalPrice.ToString("c"));

			var html = file;
			emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
			{
				Text = html
			};
		}
		return emailMessage;
	}

	private async Task SendAsync(MimeMessage mailMessage)
	{
		using var client = new SmtpClient();
		try
		{
			await client.ConnectAsync(_emailConfiguration.SmtpServer, _emailConfiguration.Port, true);
			client.AuthenticationMechanisms.Remove("XOAUTH2");
			await client.AuthenticateAsync(_emailConfiguration.Username, _emailConfiguration.Password);
			await client.SendAsync(mailMessage);
		} catch(Exception ex)
		{
			//log an error message or throw an exception or both.
			Console.WriteLine(ex.Message);
		} finally
		{
			await client.DisconnectAsync(true);
			client.Dispose();
		}
	}
}
