using Microsoft.AspNetCore.Identity;
using System.Net;
using System.Net.Mail;

namespace WebApplication.Models;

public class EmailSender : IEmailSender<ApplicationUser>
{
    private readonly IConfiguration _configuration;

    private string _mailServer;
    private string _fromEmail;
    private string _password;
    private int _port;

    public EmailSender(IConfiguration configuration)
    {
        _configuration = configuration;
        UpdateEmailConfiguration();
    }

    private void UpdateEmailConfiguration()
    {
        _mailServer = _configuration["EmailSettings:MailServer"];
        _fromEmail = _configuration["EmailSettings:FromEmail"];
        _password = _configuration["EmailSettings:Password"];
        _port = int.Parse(_configuration["EmailSettings:MailPort"]);
    }

    private async Task SendEmailAsync(string email, string subject, string body)
    {
        var client = new SmtpClient(_mailServer, _port)
        {
            Credentials = new NetworkCredential(_fromEmail, _password),
            EnableSsl = true
        };

        var mailMessage = new MailMessage(_fromEmail, email)
        {
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };

        await client.SendMailAsync(mailMessage);
    }

    public Task SendConfirmationLinkAsync(ApplicationUser user, string email, string confirmationLink)
        => SendEmailAsync(email, "Confirm your email", $"Hello {user.UserName}. Please confirm your email: <a href=\"{confirmationLink}\">CONFIRM</a>");

    public Task SendPasswordResetCodeAsync(ApplicationUser user, string email, string resetCode) 
        => throw new NotImplementedException();

    public Task SendPasswordResetLinkAsync(ApplicationUser user, string email, string resetLink)
        => SendEmailAsync(email, "Password reset link", $"Hello {user.UserName}. If you requested password reset, visit the following link: <a href=\"{resetLink}\">RESET</a>");
}
