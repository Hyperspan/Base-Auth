using Base.Api;
using Base.Services;
using Base.Shared.Config;
using RamsonDevelopers.UtilEmail;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

IConfiguration config = builder.Configuration;
var appConfiguration = config.GetSection(AppConfiguration.Label);
var connectionStrings = config.GetSection(ConnectionString.Label).Get<ConnectionString>();
var emailConfig = config.GetSection(EmailConfig.SectionLabel);


builder.Services.AddOptions();
builder.Services.Configure<AppConfiguration>(appConfiguration);
builder.Services.Configure<EmailConfig>(emailConfig);
builder.Services.AddEmailService();

builder.Services.AddBaseServices(connectionStrings.PgDatabase);
builder.Services.AddAppIdentity<Guid>();
builder.Services.AddJwtAuthentication(appConfiguration.Get<AppConfiguration>());

builder.Services.AddCors(cors =>
{
    cors.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseCors();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
