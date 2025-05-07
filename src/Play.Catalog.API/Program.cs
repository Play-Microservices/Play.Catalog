using Microsoft.AspNetCore.Authentication.JwtBearer;
using Play.Catalog.API.Entites;
using Play.Common.MassTransit;
using Play.Common.MongoDB;
using Play.Common.Settings;

const string AllowedOriginSetting = "AllowedOrigin";

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.

builder.AddMongo()
       .AddMongoRepository<Item>("items")
       .AddMassTransitWithRabbitMQ();

var serviceSettings = builder.Configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>()!;
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "https://localhost:5003";
        options.Audience = servic eSettings.ServiceName;
    });

builder.Services.AddControllers(options =>
{
    options.SuppressAsyncSuffixInActionNames = false;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseCors(builder => 
    {
        builder.WithOrigins(configuration[AllowedOriginSetting]!)
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
