using EventBus.Base;
using EventBus.Base.Abstraction;
using EventBus.Factory;
using EventBus.UnitTest.Events.Events;
using EventBus.UnitTest.Events.EventHandlers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace EventBus.UnitTest
{
    [TestClass]
    public class EventBusTests
    {
        private ServiceCollection services;

        public EventBusTests()
        {
            services= new ServiceCollection();
            services.AddLogging(configure => configure.AddConsole());
        }
        [TestMethod]
        public void subscribe_event_on_rabbitmq_test()
        {
             services.AddSingleton<IEventBus>(sp =>
            {

               
                return EventBusFactory.Create(GetRabbitMQConfig(), sp);

            });
            var sp = services.BuildServiceProvider();
            var eventBus = sp.GetRequiredService<IEventBus>();
            eventBus.Subscribe<OrderCreatedIntegrationEvent, OrderCreatedIntegrationEventHandler>();
            //eventBus.Unsubscribe<OrderCreatedIntegrationEvent,OrderCreatedIntegrationEventHandler>();
        }
        [TestMethod]
        public void send_message_to_rabbitmq()
        {
            services.AddSingleton<IEventBus>(sp =>
            {

                
                return EventBusFactory.Create(GetRabbitMQConfig(), sp);

            });
            var sp = services.BuildServiceProvider();
            var eventBus = sp.GetRequiredService<IEventBus>();

            eventBus.Publish(new OrderCreatedIntegrationEvent(1));
        }
        private EventBusConfig GetRabbitMQConfig()
        {
            return new EventBusConfig ()
            {
                ConnectionRetryCount = 5,
                SubscriberClientAppName = "EventBus.UnitTest",
                DefaultTopicName = "SellingBuddyTopicName",
                EventBusType = EventBusType.RabbitMQ,
                EventNameSuffix = "IntegrationEvent",
                Connection = new ConnectionFactory()
                {

                    //HostName = "localhost",
                    ////Port = 5672,
                    //UserName = "guest",
                    //Password = "guest"
                }
            };
        }
    }
}