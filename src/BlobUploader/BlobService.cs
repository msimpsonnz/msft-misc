using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Diagnostics;

namespace BlobUploader
{
    public class BlobService : IHostedService, IDisposable
    {
        private readonly ILogger _logger;
        private readonly IOptions<AppConfig> _appConfig;

        private static HttpClient client = new HttpClient();

        public BlobService(ILogger<BlobService> logger, IOptions<AppConfig> appConfig)
        {
            _logger = logger;
            _appConfig = appConfig;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Blob Service is starting.");

            string storageAccount = _appConfig.Value.StorageAccount;
            string storageKey = _appConfig.Value.StorageKey;
            string storageContainer = _appConfig.Value.StorageContainer;
            string sas = _appConfig.Value.Sas;

            try
            {
                foreach (var storageUrl in _appConfig.Value.StorageUrl)
                {


                    string uri = $"{storageUrl}/{storageContainer}/{Guid.NewGuid()}.json{sas}";
                    string sampleContent = "{\"name\":\"John Doe\",\"age\":33}";
                    int contentLength = Encoding.UTF8.GetByteCount(sampleContent);
                    string now = DateTime.Now.ToString("R", CultureInfo.InvariantCulture);

                    var request = new HttpRequestMessage(HttpMethod.Put, uri);


                    request.Content = new StringContent(sampleContent, Encoding.UTF8, "application/json");
                    request.Content.Headers.ContentLength = contentLength;

                    request.Headers.Add("x-ms-version", "2018-03-28");
                    request.Headers.Add("x-ms-date", now);
                    request.Headers.Add("x-ms-blob-type", "BlockBlob");

                    var timer = Stopwatch.StartNew();
                    var req = await new HttpClient().SendAsync(request);
                    timer.Stop();
                    var response = req.StatusCode.ToString();
                    _logger.LogInformation($"Request: {storageUrl}, ResponseTime: {timer.Elapsed}");
                }
                return;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to execute application: {ex.Message}");
            }

            return;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Service is stopping.");

            return Task.CompletedTask;
        }

        public void Dispose()
        {

        }
    }
}
