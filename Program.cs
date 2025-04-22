using BlazorMailKitApp.Components;
using BlazorMailKitApp.Services;
using MailKit;

var builder = WebApplication.CreateBuilder(args);

// Add configuration from environment variables
builder.Configuration.AddEnvironmentVariables();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// builder.Services.AddSingleton<GmailApiService>();
// builder.Services.AddTransient<GmailSmtpService>();

// Register GmailApiService or GmailSmtpService
builder.Services.AddScoped<IGmailService, GmailApiService>();
// Or switch to GmailSmtpService
// builder.Services.AddScoped<IGmailService, GmailSmtpService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
