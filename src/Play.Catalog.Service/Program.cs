using MassTransit;
using Play.Catalog.Service.Entites;
using Play.Catalog.Service.Settings;
using Play.Common.MongoDB;
using Play.Common.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.AddMongo()
       .AddMongoRepository<Item>("items");

var serviceSettings = builder.Configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();
builder.Services.AddMassTransit(x => 
{
    x.UsingRabbitMq((context, configurator) => 
    {
        var rabbitMQSettings = builder.Configuration.GetSection(nameof(RabbitMQSettings)).Get<RabbitMQSettings>();
        configurator.Host(rabbitMQSettings!.Host);
        configurator.ConfigureEndpoints(context, new KebabCaseEndpointNameFormatter(serviceSettings!.ServiceName, false));
    });
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
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
