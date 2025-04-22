using System;
using System.Net;
using System.Net.Mail;

namespace BlazorMailKitApp.Services;

public class GmailSmtpService(IConfiguration _config) : IGmailService
{

    public async Task SendEmailAsync(string receptor, string subject, string body)
    {
        var email = _config.GetValue<string>("EMAIL_CONFIGURATION:EMAIL");
        var password = _config.GetValue<string>("EMAIL_CONFIGURATION:PASSWORD");
        var host = _config.GetValue<string>("EMAIL_CONFIGURATION:HOST");
        var port = _config.GetValue<int>("EMAIL_CONFIGURATION:PORT");

        Console.WriteLine($"Email: {email}");
        Console.WriteLine($"Password: {password}");
        Console.WriteLine($"Host: {host}");
        Console.WriteLine($"Port: {port}");
        Console.WriteLine($"Receptor: {receptor}");
        Console.WriteLine($"Subject: {subject}");
        Console.WriteLine($"Body: {body}");

        var smtpClient = new SmtpClient(host, port);
        smtpClient.EnableSsl = true;
        smtpClient.UseDefaultCredentials = false;

        smtpClient.Credentials = new NetworkCredential(email, password);
        var message = new MailMessage(email!, receptor, subject, body);
        await smtpClient.SendMailAsync(message);
    }

}
