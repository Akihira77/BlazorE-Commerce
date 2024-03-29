﻿using BlazorEcommerce.Server.Features.Base;
using BlazorEcommerce.Server.StaticFiles.Invoice;
using BlazorEcommerce.Server.StaticFiles.OrderNotification;

using MailKit.Net.Smtp;
using MimeKit;
using System.Text;

namespace BlazorEcommerce.Server.Features.Emails;

public class EmailSender : IEmailSender
{
    private readonly EmailConfiguration _emailConfiguration;
	private readonly IWebHostEnvironment _webHostEnvironment;
	private readonly IUnitOfWork _unitOfWork;
    private readonly string _directoryPath;

    public EmailSender(
        IUnitOfWork unitOfWork,
        EmailConfiguration emailConfiguration,
        IWebHostEnvironment webHostEnvironment)
    {
        _unitOfWork = unitOfWork;
		_emailConfiguration = emailConfiguration;
		_webHostEnvironment = webHostEnvironment;
		_directoryPath = Directory.GetCurrentDirectory();
    }

    public async Task<bool> SendEmailAsync(Message message)
    {
        var emailMessage = new MimeMessage();
        if (message.Subject.ToLower().Equals("invoice"))
        {
            emailMessage = await CreateInvoiceMessage(message);
        }
        else if (message.Subject.ToLower().Equals("checkout"))
        {
            emailMessage = await CreateEmailMessage(message);
        }
        else if (message.Subject.ToLower().Equals("otp"))
        {
            emailMessage = CreateOTPMessage(message);
        }
        return await SendAsync(emailMessage);
    }

    private MimeMessage CreateOTPMessage(Message message)
    {
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress("Blazor ECommerce - OTP", _emailConfiguration.From));
        emailMessage.To.AddRange(message.To);

        emailMessage.Subject = "One Time Password";
        emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
        {
            Text = $"<p>This is your OTP <span style='font-weight: 600'>{message.Content}</span></p>" +
                $"<br /> <p>This OTP can be use just for <span style='font-weight: 600'>3 minutes</span></p>"
        };

        return emailMessage;
    }

    private async Task<MimeMessage> CreateInvoiceMessage(Message message)
    {
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress("Blazor ECommerce", _emailConfiguration.From));
        emailMessage.To.AddRange(message.To);

        emailMessage.Subject = "Blazor ECommerce Order Notification";

        var order = await _unitOfWork.Order.GetOrderDetails(int.Parse(message.Content));
        var fileName = "InvoiceTop.html";
        //var file = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "StaticFiles", "Html", fileName));
        //var file = File.ReadAllText(Path.Combine(_directoryPath, "StaticFiles", "Html", fileName));
        var file = Invoice.InvoiceText;
        var htmlOrderItems = "<tr class=\"item\">\r\n    <td>{0}</td>\r\n\r\n    <td>{1}</td>\r\n</tr>\r\n\r\n";

        var sb = new StringBuilder();
        foreach (var item in order.Products)
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
        return emailMessage;
    }

    private async Task<MimeMessage> CreateEmailMessage(Message message)
    {
        var userEmail = message.To[0].Address;
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress("Transaction Successful", userEmail));
        emailMessage.To.Add(new MailboxAddress("Blazor ECommerce", _emailConfiguration.From));

        emailMessage.Subject = message.Subject;
		var fileName = "OrderNotification.html";
        //var file = File.ReadAllText(Path.Combine(_webHostEnvironment.ContentRootPath, "StaticFiles", "Html", fileName));
        //var file = File.ReadAllText(Path.Combine(_directoryPath, "StaticFiles", "Html", fileName));
        var file = OrderNotification.OrderNotificationText;
        file = file.Replace("{{0}}", message.Content);

        emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
        {
            Text = file
        };

        return emailMessage;
    }

    private async Task<bool> SendAsync(MimeMessage mailMessage)
    {
        using var client = new SmtpClient();
        try
        {
            await client.ConnectAsync(_emailConfiguration.SmtpServer, _emailConfiguration.Port, true);
            client.AuthenticationMechanisms.Remove("XOAUTH2");
            await client.AuthenticateAsync(_emailConfiguration.Username, _emailConfiguration.Password);
            await client.SendAsync(mailMessage);
            return true;
        }
        catch (Exception ex)
        {
            //log an error message or throw an exception or both.
            return false;
        }
        finally
        {
            await client.DisconnectAsync(true);
            client.Dispose();
        }
    }
}