using EventBus.Base.Abstraction;
using EventBus.Base;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using EventBus.Factory;
using NotificationService.EventHandler;
using NotificationService.Api.IntegrationEvents.Events;

namespace NotificationService
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Dependecy Injection yapabilmek için "ServiceCollection class'ına ihtiyaç vardır."
            ServiceCollection services = new ServiceCollection();

            ConfigureServices(services);

            var sp = services.BuildServiceProvider();

            IEventBus eventBus = sp.GetRequiredService<IEventBus>();
            //Burada  yer alan  kuyrukları dinlemek istiyorum.
            eventBus.Subscribe<OrderPaymentSuccessIntegrationEvent, OrderPaymentSuccessIntegrationEventHandler>();
            eventBus.Subscribe<OrderPaymentFailedIntegrationEvent, OrderPaymentFailedIntegrationEventHandler>();


            Console.WriteLine("Application is running");
            Console.ReadLine();
        }

        private static void ConfigureServices(ServiceCollection services)
        {

            services.AddLogging(configure =>
            {
                configure.AddConsole();
              
            });
            //Dinlenecek eventler, Bu eventlerde her hangi bir işlem olursa ilgili handler metodları çalışacak.
            services.AddTransient<OrderPaymentFailedIntegrationEventHandler>();
            services.AddTransient<OrderPaymentSuccessIntegrationEventHandler>();

            services.AddSingleton<IEventBus>(sp =>
            {
                EventBusConfig config = new()
                {
                    ConnectionRetryCount = 5,
                    EventNameSuffix = "IntegrationEvent",
                    SubscriberClientAppName = "NotificationService",
                    EventBusType = EventBusType.RabbitMQ,
                    
                };

                return EventBusFactory.Create(config, sp);
            });

        }


    }
}