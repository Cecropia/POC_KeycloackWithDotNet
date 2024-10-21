using KeyCloakIntegration.Infrastructure.Data;
using KeyCloakIntegration.Web;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var keycloakSettings = builder.Configuration.GetSection("Authentication:Keycloak");

// Add services to the container.
builder.Services.AddKeyVaultIfConfigured(builder.Configuration);

// Add Application, Infrastructure, and Web services.
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
IServiceCollection serviceCollection = builder.Services.AddWebServices(builder.Configuration);

// Configure Keycloak Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})

.AddJwtBearer(options =>
{
    options.Authority = keycloakSettings["Authority"];
    options.Audience = keycloakSettings["Audience"];
    options.RequireHttpsMetadata = false; // Set to true if using HTTPS in production
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = keycloakSettings["Authority"],
        ValidateAudience = true,
        ValidAudience = keycloakSettings["Audience"],
        ValidateLifetime = true
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    await app.InitialiseDatabaseAsync();
}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHealthChecks("/health");
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSwaggerUi(settings =>
{
    settings.Path = "/api";
    settings.DocumentPath = "/api/specification.json";
});

// Use Authentication and Authorization middleware
app.UseAuthentication();  // This ensures the app uses the authentication scheme configured above
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapRazorPages();
app.MapFallbackToFile("index.html");

// Error handling
app.UseExceptionHandler(options => { });

app.MapEndpoints();

app.Run();

public partial class Program { }

