using BasketService.Api.Core.Application.Repository;
using BasketService.Api.Core.Application.Services;
using BasketService.Api.Extensions;
using BasketService.Api.Extensions.Registration;
using BasketService.Api.Infrastructure.Repository;
using BasketService.Api.IntegrationEvents.EventHanders;
using BasketService.Api.IntegrationEvents.Events;
using EventBus.Base;
using EventBus.Base.Abstraction;
using EventBus.Factory;
using Microsoft.AspNetCore.Connections;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



#region ServiceConfigurations
//Configure Consul
builder.Services.ConfigureConsul(builder.Configuration);

//Configure JWT
builder.Services.ConfigureAuth(builder.Configuration);

//Configure Redis
builder.Services.AddSingleton(sp=>sp.ConfigureRedis(builder.Configuration));
//HttpAccessor
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IBasketRepository,RedisBasketRepository>();

builder.Services.AddTransient<IIdentityService, IdentityService>();
#endregion

#region IEventBusConfiguration
builder.Services.AddTransient<OrderCreatedIntegrationEventHandler>();

builder.Services.AddSingleton<IEventBus>(sp =>
{
    EventBusConfig config = new()
    {
        ConnectionRetryCount = 5,
        EventNameSuffix = "IntegrationEvent",
        SubscriberClientAppName = "BasketService",
        EventBusType = EventBusType.RabbitMQ,
       
    };

    return EventBusFactory.Create(config, sp);
});

#endregion
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

//sistem ?zerinde bir tane IEventBus create edecek ve sat?r 44-58 sat?r ?al??acak.
IEventBus eventBus = app.Services.GetRequiredService<IEventBus>();
//event bus' a diyoruz ki bizim i?in "OrderCreatedIntegrationEvent" i  dinlemeye ba?la ve dinledikte sonra  OrderCreatedIntegrationEventHandler i?erisindekileri yap!
eventBus.Subscribe<OrderCreatedIntegrationEvent, OrderCreatedIntegrationEventHandler>();

#region AppConsul

app.Start(); // RegisterWithConsul i?erisinde host ve port de?erlerini almak i?in eklendi
//Application Register to consul.
app.RegisterWithConsul(app.Lifetime);
app.WaitForShutdown(); // RegisterWithConsul i?erisinde host ve port de?erlerini almak i?in eklendi
#endregion

app.Run();

