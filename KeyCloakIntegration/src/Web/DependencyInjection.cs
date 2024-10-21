using Azure.Identity;
using KeyCloakIntegration.Application.Common.Interfaces;
using KeyCloakIntegration.Infrastructure.Data;
using KeyCloakIntegration.Web.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc;
using System;

namespace KeyCloakIntegration.Web
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddWebServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Add database-related services
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddScoped<IUser, CurrentUser>();

            services.AddHttpContextAccessor();

            services.AddHealthChecks()
                .AddDbContextCheck<ApplicationDbContext>();

            services.AddExceptionHandler<CustomExceptionHandler>();

            services.AddRazorPages();

            // Customize default API behavior
            services.Configure<ApiBehaviorOptions>(options =>
                options.SuppressModelStateInvalidFilter = true);

            services.AddEndpointsApiExplorer();

            services.AddOpenApiDocument((configure, sp) =>
            {
                configure.Title = "KeyCloakIntegration API";
            });

            // Load Keycloak configuration from appsettings.json
            var keycloakSettings = configuration.GetSection("Authentication:Keycloak");

            // Add Authentication with Keycloak
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.Authority = keycloakSettings["Authority"];
                options.Audience = keycloakSettings["Audience"];
                options.RequireHttpsMetadata = false; // Use true if HTTPS is required in production
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = keycloakSettings["Authority"],
                    ValidateAudience = true,
                    ValidAudience = keycloakSettings["Audience"],
                    ValidateLifetime = true
                };
            });

            // Add Authorization
            services.AddAuthorization();

            return services;
        }

        public static IServiceCollection AddKeyVaultIfConfigured(this IServiceCollection services, ConfigurationManager configuration)
        {
            var keyVaultUri = configuration["AZURE_KEY_VAULT_ENDPOINT"];
            if (!string.IsNullOrWhiteSpace(keyVaultUri))
            {
                configuration.AddAzureKeyVault(
                    new Uri(keyVaultUri),
                    new DefaultAzureCredential());
            }

            return services;
        }
    }
}
