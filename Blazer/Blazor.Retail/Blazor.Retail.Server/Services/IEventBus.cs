using Blazor.Retail.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazor.Retail.Server.Services
{
    public interface IEventBus
    {
        Task Publish(Product @event);
    }
}
