using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using VMTS.Core.Helpers;
using VMTS.Core.Interfaces.Services;

namespace VMTS.Service.Services;

public class EmailService : IEmailService
{
    private readonly IOptions<EmailSettings> _settings;

    public EmailService(IOptions<EmailSettings> settings)
    {
        _settings = settings;
    }

    public async Task SendEmailAsync(
        string toEmail,
        string subject,
        string body,
        bool isHtml = true
    )
    {
        var email = new MimeMessage();
        email.From.Add(new MailboxAddress(_settings.Value.SenderName, _settings.Value.SenderEmail));
        email.To.Add((MailboxAddress.Parse(toEmail)));
        email.Subject = subject;

        var builder = new BodyBuilder();

        if (isHtml)
            builder.HtmlBody = body;
        else
            builder.TextBody = body;

        using var stmp = new SmtpClient();
        await stmp.ConnectAsync(
            _settings.Value.SmtpServer,
            _settings.Value.Port,
            SecureSocketOptions.StartTls
        );
        await stmp.AuthenticateAsync(_settings.Value.Username, _settings.Value.Password);
        await stmp.SendAsync(email);
        await stmp.DisconnectAsync(true);
    }
}
