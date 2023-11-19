using Hyperspan.Auth.Domain.DatabaseModals;
using Hyperspan.Base;
using Hyperspan.Shared.Config;
using Microsoft.AspNetCore.Identity;
using System;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddAuthorization();
builder.Services.AddBaseApi(builder.Configuration);

var connectionSection = builder.Configuration.GetSection(ConnectionString.Label);
builder.Services.Configure<ConnectionString>(connectionSection);


// Identity configuration
builder.Services
    .AddIdentity<ApplicationUser<Guid>, ApplicationRole<Guid>>(options =>
    {
        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.User.RequireUniqueEmail = true;
    })
    .AddRoles<ApplicationRole<Guid>>()
    .AddEntityFrameworkStores<DbContext>()
    .AddDefaultTokenProviders();


var app = builder.Build();

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});
app.MapFallbackToFile("index.html");
//app.MapControllers();
app.Run();





