using EventBus.Base.Abstraction;
using Microsoft.Extensions.Logging;
using NotificationService.Api.IntegrationEvents.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.EventHandler
{
    public class OrderPaymentSuccessIntegrationEventHandler : IIntegrationEventHandler<OrderPaymentSuccessIntegrationEvent>
    {
        private readonly ILogger<OrderPaymentSuccessIntegrationEvent> logger;

        public OrderPaymentSuccessIntegrationEventHandler(ILogger<OrderPaymentSuccessIntegrationEvent> logger)
        {
            this.logger = logger;
        }
        public Task Handle(OrderPaymentSuccessIntegrationEvent @event)
        {
            logger.LogInformation($"Order Payment Success with OrderId: {@event.OrderId}");

            return Task.CompletedTask;
        }
    }
}
