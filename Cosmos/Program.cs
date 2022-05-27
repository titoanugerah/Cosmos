using Cosmos.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Serilog;

var builder = WebApplication.CreateBuilder(args);


Config config = new Config();

Serilog.Core.Logger logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Logging
    .ClearProviders();

builder.Logging
    .AddSerilog(logger);

//Get config
config.Authentication = builder.Configuration.GetSection("Authentication").Get<Authentication>();

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

//Authentication
builder.Services.AddAuthentication( options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = OpenIdConnectDefaults.AuthenticationScheme;
}).AddOpenIdConnect("OpenIdConnect", options =>
{
    options.Authority = config.Authentication.Authority;
    options.ClientId = config.Authentication.ClientId;
    options.ClientSecret = config.Authentication.ClientSecret;
    foreach (string scope in config.Authentication.Scopes)
    {
        options.Scope.Add(scope);
    }
    options.ResponseType = OpenIdConnectResponseType.CodeIdToken;
    options.CallbackPath = config.Authentication.CallbackPath;
    options.SaveTokens = config.Authentication.SaveTokens;
    options.GetClaimsFromUserInfoEndpoint = config.Authentication.GetClaimsFromUserInfoEndpoint;
    options.RequireHttpsMetadata = config.Authentication.RequireHttpsMetadata;
}).AddCookie( options =>
{
    //options.Cookie.Expiration = TimeSpan.FromHours(config.Authentication.CookieExpiration);
    options.ExpireTimeSpan = TimeSpan.FromHours(config.Authentication.ExpireTimeSpan);
    options.SlidingExpiration = config.Authentication.SlidingExpiration;
});

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
