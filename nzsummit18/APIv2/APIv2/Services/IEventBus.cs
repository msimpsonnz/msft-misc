using System;
using System.Threading.Tasks;
using APIv2.Models;

namespace APIv2.Services
{
    public interface IEventBus
    {
        Task Publish(Product @event);
    }
}
