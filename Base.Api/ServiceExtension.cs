using System.Net;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using RamsonDevelopers.UtilEmail;
using Hyperspan.Base.Shared.Config;
using Hyperspan.Auth.Domain.DatabaseModals;
using Hyperspan.Base.Shared.Modals;
using Hyperspan.Base.Database;
using Hyperspan.Base.Services;
using Hyperspan.Settings.Api;
using Hyperspan.Settings.Services;

namespace Hyperspan.Base.Api
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddBaseApi(this IServiceCollection services, IConfiguration config)
        {
            var appConfiguration = config.GetSection(AppConfiguration.Label);
            var connectionStrings = config.GetSection(ConnectionString.Label).Get<ConnectionString>();
            var emailConfig = config.GetSection(EmailConfig.SectionLabel);


            services.AddOptions();
            services.Configure<AppConfiguration>(appConfiguration);
            services.Configure<EmailConfig>(emailConfig);
            services.AddEmailService();
            services.AddBaseServices(connectionStrings.PgDatabase);
            services.AddAppIdentity<Guid>();
            services.AddJwtAuthentication(appConfiguration.Get<AppConfiguration>());
            services.AddSettingsApi();
            services.AddCors(cors =>
            {
                cors.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });

            return services;
        }


        /// <summary>
        /// Add the identity Services to help in using the ASP net Identity.
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <returns></returns>
        internal static IServiceCollection AddAppIdentity<T>(
                                        this IServiceCollection serviceCollection) where T : IEquatable<T>
        {
            serviceCollection
                .AddIdentity<ApplicationUser<T>, ApplicationRole<T>>(options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.User.RequireUniqueEmail = true;
                })
                .AddRoles<ApplicationRole<T>>()
                .AddEntityFrameworkStores<Contexts>()
                .AddDefaultTokenProviders();

            return serviceCollection;
        }

        internal static IServiceCollection AddJwtAuthentication(
            this IServiceCollection serviceCollection,
            AppConfiguration appConfig)
        {
            serviceCollection
                .AddAuthentication(authentication =>
                {
                    authentication.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    authentication.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(bearer =>
                {
                    bearer.RequireHttpsMetadata = false;
                    bearer.SaveToken = true;

                    bearer.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appConfig.JwtSecurityKey)),
                        ValidateIssuerSigningKey = true,
                        RoleClaimType = ClaimTypes.Role,
                    };
                    bearer.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = c =>
                        {
                            if (c.Exception is SecurityTokenExpiredException)
                            {
                                c.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                                c.Response.ContentType = "application/json";
                                var result = JsonSerializer.Serialize(
                                    ApiResponseModal<object>.FailedAsync("The Token is expired."));
                                return c.Response.WriteAsync(result);
                            }
                            c.NoResult();
                            c.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            c.Response.ContentType = "text/plain";
                            return c.Response.WriteAsync(c.Exception.ToString());
                        },
                        OnChallenge = context =>
                        {
                            context.HandleResponse();
                            context.Response.StatusCode = 401;
                            context.Response.ContentType = "application/json";
                            var result = JsonSerializer.Serialize(
                                ApiResponseModal<object>.FailedAsync("You have been logged out! Please login again."));
                            return context.Response.WriteAsync(result);
                        },
                        OnForbidden = context =>
                        {
                            context.Response.StatusCode = 403;
                            context.Response.ContentType = "application/json";
                            var result = JsonSerializer.Serialize(
                                ApiResponseModal<object>.FailedAsync("You are not authorized to access this resource."));
                            return context.Response.WriteAsync(result);
                        },
                    };
                });

            serviceCollection.AddHttpContextAccessor();
            return serviceCollection;
        }

    }
}
