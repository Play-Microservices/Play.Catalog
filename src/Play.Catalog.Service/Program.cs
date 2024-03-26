using Play.Catalog.Service.Entites;
using Play.Common.MassTransit;
using Play.Common.MongoDB;

const string AllowedOriginSetting = "AllowedOrigin";

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.

builder.AddMongo()
       .AddMongoRepository<Item>("items")
       .AddMassTransitWithRabbitMQ();

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

app.UseAuthorization();

app.MapControllers();

app.Run();
