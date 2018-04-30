using Microsoft.Azure.ServiceBus;
using System;

namespace Blazor.Retail.Server.Services
{
    public interface IServiceBusPersisterConnection : IDisposable
    {
        ServiceBusConnectionStringBuilder ServiceBusConnectionStringBuilder { get; }

        ITopicClient CreateModel();
    }
}
