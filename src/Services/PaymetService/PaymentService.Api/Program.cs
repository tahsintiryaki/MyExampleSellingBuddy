using EventBus.Base.Abstraction;
using EventBus.Base;
using EventBus.Factory;
using PaymentService.Api.IntegrationEvents.EventHandlers;
using RabbitMQ.Client;
using PaymentService.Api.IntegrationEvents.Events;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();




builder.Services.AddLogging(configure =>
{
    configure.AddConsole();
    configure.AddDebug();
});

builder.Services.AddTransient<OrderStartedIntegrationEventHandler>();

builder.Services.AddSingleton<IEventBus>(sp =>
{
    EventBusConfig config = new()
    {
        ConnectionRetryCount = 5,
        EventNameSuffix = "IntegrationEvent",
        SubscriberClientAppName = "PaymentService",
        EventBusType = EventBusType.RabbitMQ,
        Connection = new ConnectionFactory()
        {

            //HostName = "localhost",
            ////Port = 5672,
            //UserName = "guest",
            //Password = "guest"
        }
    };

    return EventBusFactory.Create(config, sp);
});






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
//sistem ?zerinde bir tane IEventBus create edecek ve sat?r 28-47 ?al??acak.
IEventBus eventBus = app.Services.GetRequiredService<IEventBus>();
//event bus' a diyoruz ki bizim i?in "OrderStartedIntegrationEvent" i  dinlemeye ba?la ve dinledikte sonra  OrderStartedIntegrationEventHandler i?erisindekileri yap!
eventBus.Subscribe<OrderStartedIntegrationEvent, OrderStartedIntegrationEventHandler>();

app.Run();

