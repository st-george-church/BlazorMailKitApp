using System;

namespace BlazorMailKitApp.Services;

public interface IGmailService
{
    Task SendEmailAsync(string receptor, string subject, string body);
}
