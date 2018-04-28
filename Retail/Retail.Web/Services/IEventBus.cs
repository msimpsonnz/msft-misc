using System;
using System.Threading.Tasks;
using Retail.Common.Models;

namespace Retail.Web.Services
{
    public interface IEventBus
    {
        Task Publish(Product @event);
    }
}
