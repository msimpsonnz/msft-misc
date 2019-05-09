using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Newtonsoft.Json;

namespace monitorService
{
    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    internal sealed class monitorService : StatelessService
    {
        private static HttpClient httpClient = new HttpClient();
        public monitorService(StatelessServiceContext context)
            : base(context)
        {
        }

        /// <summary>
        /// Optional override to create listeners (e.g., TCP, HTTP) for this service replica to handle client or user requests.
        /// </summary>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return new ServiceInstanceListener[0];
        }

        /// <summary>
        /// This is the main entry point for your service instance.
        /// </summary>
        /// <param name="cancellationToken">Canceled when Service Fabric needs to shut down this service instance.</param>
        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            // TODO: Replace the following sample code with your own logic 
            //       or remove this RunAsync override if it's not needed in your service.


            long iterations = 0;

            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();


                var monitor = await httpClient.GetStringAsync("https://mcapi.us/server/status?ip=104.211.11.154&port=25575");

                var result = JsonConvert.DeserializeObject<MinecraftStatus>(monitor);

                ServiceEventSource.Current.ServiceMessage(this.Context, $"Status: {result.status}, PlayersMax: {result.players.max}, PlayersCurrent: {result.players.now}", ++iterations);


                await Task.Delay(TimeSpan.FromSeconds(60), cancellationToken);
            }
        }
    }

    public class MinecraftStatus
    {
        public string status { get; set; }
        public bool online { get; set; }
        public string motd { get; set; }
        public string error { get; set; }
        public Players players { get; set; }
        public Server server { get; set; }
        public string last_online { get; set; }
        public string last_updated { get; set; }
        public int duration { get; set; }
    }

    public class Players
    {
        public int max { get; set; }
        public int now { get; set; }
    }

    public class Server
    {
        public string name { get; set; }
        public int protocol { get; set; }
    }

}
