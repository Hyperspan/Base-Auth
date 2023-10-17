using Auth.Domain.DatabaseModals;
using Base.Api;
using Base.Services;
using Base.Shared.Config;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

IConfiguration config = builder.Configuration;
var appConfiguration = config.GetSection(AppConfiguration.Label);
var connectionStrings = config.GetSection(ConnectionString.Label).Get<ConnectionString>();


builder.Services.AddOptions();
builder.Services.Configure<AppConfiguration>(appConfiguration);

builder.Services.AddBaseServices(connectionStrings.PgDatabase);
builder.Services.AddAppIdentity<Guid>();
builder.Services.AddJwtAuthentication(appConfiguration.Get<AppConfiguration>());


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
