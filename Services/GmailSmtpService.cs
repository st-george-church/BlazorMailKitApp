using System;
using System.Net;
using System.Net.Mail;

namespace BlazorMailKitApp.Services;

public class GmailSmtpService(IConfiguration _config)
{

    public async Task SendEmailAsync(string receptor, string subject, string body, bool isHtml = false)
    {
        var fromEmail = _config.GetValue<string>("GmailSmtpConfig:FromEmail");
        var fromName = _config.GetValue<string>("GmailSmtpConfig:FromName");
        var password = _config.GetValue<string>("GmailSmtpConfig:Password");
        var host = _config.GetValue<string>("GmailSmtpConfig:Host");
        var port = _config.GetValue<int>("GmailSmtpConfig:Port");

        // Console.WriteLine($"Email: {fromEmail}");
        // Console.WriteLine($"Password: {password}");
        // Console.WriteLine($"Host: {host}");
        // Console.WriteLine($"Port: {port}");
        // Console.WriteLine($"Receptor: {receptor}");
        // Console.WriteLine($"Subject: {subject}");
        // Console.WriteLine($"Body: {body}");

        var smtpClient = new SmtpClient(host, Convert.ToInt16(port));
        smtpClient.EnableSsl = true;
        smtpClient.UseDefaultCredentials = false;

        smtpClient.Credentials = new NetworkCredential(fromEmail, password);
        // var message = new MailMessage(fromEmail!, receptor, subject, body);
        
        var message = new MailMessage
        {
            From = new MailAddress(fromEmail!, fromName),
            Subject = subject,
            Body = body,
            To = { new MailAddress(receptor) },
            IsBodyHtml = isHtml
        };
        
        await smtpClient.SendMailAsync(message);
    }

}
