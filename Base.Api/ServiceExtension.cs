using System.Net;
using Auth.Domain.DatabaseModals;
using Base.Shared.Config;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Base.Database;
using Base.Shared.Modals;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Base.Api
{
    public static class ServiceExtension
    {

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
                        ValidateIssuer = true,
                        ValidateAudience = true,
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
