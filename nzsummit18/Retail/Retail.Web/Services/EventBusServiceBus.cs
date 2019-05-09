using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Retail.Common.Models;

namespace Retail.Web.Services
{
    public class EventBusServiceBus : IEventBus
    {
        private readonly IServiceBusPersisterConnection _serviceBusPersisterConnection;

        public EventBusServiceBus(IServiceBusPersisterConnection serviceBusPersisterConnection)
        {
            _serviceBusPersisterConnection = serviceBusPersisterConnection;
        }

        public async Task Publish(Product @event)
        {
            var jsonMessage = JsonConvert.SerializeObject(@event);
            var body = Encoding.UTF8.GetBytes(jsonMessage);

            var message = new Message
            {
                MessageId = Guid.NewGuid().ToString(),  
                Body = body
            };

            var topicClient = _serviceBusPersisterConnection.CreateModel();

            //topicClient.SendAsync(message)
            //    .GetAwaiter()
            //    .GetResult();
            await topicClient.SendAsync(message);
        }
    }
}
