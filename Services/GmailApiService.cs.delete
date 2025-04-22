using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using MimeKit;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using System.Threading;

namespace BlazorMailKitApp.Services;

public class GmailApiService
{
    private const string ApplicationName = "BlazorMailKitApp";
    private readonly GmailService _gmailService;
    private readonly IConfiguration _configuration;
    public GmailApiService(IConfiguration configuration)
    {
        _configuration = configuration;
        var credential = Authenticate();
        _gmailService = new GmailService(new BaseClientService.Initializer
        {
            HttpClientInitializer = credential,
            ApplicationName = ApplicationName,
        });
    }

    private UserCredential Authenticate()
    {
        GetGoogleCredentials();
        try
        {
            var googleCredentials = GetGoogleCredentials();

            if (googleCredentials == null)
            {
                Console.WriteLine("GoogleCredentials section is missing in appsettings.Development.json.");
                throw new Exception("Google credentials are missing or incomplete in appsettings.Development.json.");
            }

            /* Debug: Log the retrieved credentials */
            // Console.WriteLine($"Client ID: {googleCredentials.client_id}");
            // Console.WriteLine($"Project ID: {googleCredentials.project_id}");
            // Console.WriteLine($"Auth URI: {googleCredentials.auth_uri}");
            // Console.WriteLine($"Token URI: {googleCredentials.token_uri}");
            // Console.WriteLine($"Auth Provider X509 Cert URL: {googleCredentials.auth_provider_x509_cert_url}");
            // Console.WriteLine($"Client Secret: {googleCredentials.client_secret}");
            // Console.WriteLine($"Redirect URIs: {string.Join(", ", googleCredentials.redirect_uris ?? new List<string>())}");
            // Console.WriteLine($"Javascript Origins: {string.Join(", ", googleCredentials.javascript_origins ?? new List<string>())}");

            // Serialize GoogleCredentials to JSON
            var googleCredentialsJson = JsonConvert.SerializeObject(new
            {
                installed = googleCredentials
            });

            /* Debug: Log the serialized JSON */
            // Console.WriteLine($"Serialized Google Credentials JSON: {googleCredentialsJson}");

            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(googleCredentialsJson));
            string credPath = "TokenStore";

            var redirectUri = googleCredentials.redirect_uris?.FirstOrDefault() ?? "https://localhost:7268/oauth2callback";

            // Authorize using GoogleWebAuthorizationBroker
            return GoogleWebAuthorizationBroker.AuthorizeAsync(
                GoogleClientSecrets.FromStream(stream).Secrets,
                new[] { GmailService.Scope.GmailSend },
                "user",
                CancellationToken.None,
                new FileDataStore(credPath, true),
                new LocalServerCodeReceiver(redirectUri)
            ).Result;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during authentication: {ex.Message}");
            throw;
        }
    }
    public async Task SendEmailAsync(string to, string subject, string body)
    {
        var email = new MimeMessage();
        // read next line values from appsettings.json
        var fromEmail = _configuration["EmailSettings:FromEmail"];
        var fromName = _configuration["EmailSettings:FromName"];
        email.From.Add(new MailboxAddress(fromName, fromEmail));
        email.To.Add(new MailboxAddress("medhat@mobiware.ca", to));
        email.Subject = subject;

        email.Body = new TextPart("plain")
        {
            Text = body
        };

        using var memoryStream = new MemoryStream();
        email.WriteTo(memoryStream);
        var rawMessage = Convert.ToBase64String(memoryStream.ToArray())
            .Replace('+', '-')
            .Replace('/', '_')
            .Replace("=", "");

        var message = new Message
        {
            Raw = rawMessage
        };

        await _gmailService.Users.Messages.Send(message, "me").ExecuteAsync();
    }

    public GoogleCredentials? GetGoogleCredentials()
    {
        GoogleCredentials googleCredentials = new GoogleCredentials()
        {
            client_id = _configuration["GoogleCredentials:web:client_id"],
            project_id = _configuration["GoogleCredentials:web:project_id"],
            auth_uri = _configuration["GoogleCredentials:web:auth_uri"],
            token_uri = _configuration["GoogleCredentials:web:token_uri"],
            auth_provider_x509_cert_url = _configuration["GoogleCredentials:web:auth_provider_x509_cert_url"],
            client_secret = _configuration["GoogleCredentials:web:client_secret"],
            redirect_uris = _configuration.GetSection("GoogleCredentials:web:redirect_uris").Get<List<string>>(),
            javascript_origins = _configuration.GetSection("GoogleCredentials:web:javascript_origins").Get<List<string>>(),
        };

        return googleCredentials;
    }
}

public class GoogleCredentials
{
    public string? client_id { get; set; }
    public string? project_id { get; set; }
    public string? auth_uri { get; set; }
    public string? token_uri { get; set; }
    public string? auth_provider_x509_cert_url { get; set; }
    public string? client_secret { get; set; }
    public List<string>? redirect_uris { get; set; }
    public List<string>? javascript_origins { get; set; }
}