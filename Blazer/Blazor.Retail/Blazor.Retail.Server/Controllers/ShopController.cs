using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Dapper;
using System.Data;
using System.Data.SqlClient;

using Blazor.Retail.Server.Services;
using Blazor.Retail.Shared.Models;
using Microsoft.Extensions.Caching.Memory;

namespace Blazor.Retail.Server.Controllers
{
    [Route("api/[controller]")]
    public class ShopController : Controller
    {
        private static HttpClient httpClient = new HttpClient();
        private readonly IConfiguration _configuration;
        private readonly IFeatureFlag _feature;
        private static int failCounter;
        private IMemoryCache _cache;
        private IEventBus _eventBus;

        public ShopController(IConfiguration configuration, IFeatureFlag feature, IMemoryCache cache, IEventBus eventBus)
        {
            _configuration = configuration;
            _feature = feature;
            _cache = cache;
            _eventBus = eventBus;
        }

        [HttpGet("[action]")]
        public IActionResult GetAllProducts()
        {
            //Failure counter for random failure
            failCounter++;
            //Check if failure feature is enabled and fail every 'n' times
            if (_feature.IsFeatureEnabled("enableFailures") && failCounter % _configuration.GetValue<int>("Failures:failureRate") == 0)
            {
                using (IDbConnection dbConnection = new SqlConnection(_configuration["Sql"]))
                {
                    dbConnection.Open();
                    return Ok(dbConnection.Query<Product>("SELECT TOP 10 ProductId, Segment, Category, Description, ProductPrice FROM Product ORDER BY Product DESC"));
                }
            }
            var cacheKey = "products";
            var cacheEntry = _cache.GetOrCreate(cacheKey, entry =>
            {
                entry.SlidingExpiration = (TimeSpan.FromHours(1));

                using (IDbConnection dbConnection = new SqlConnection(_configuration["Sql"]))
                {
                    dbConnection.Open();
                    return dbConnection.Query<Product>("SELECT TOP 10 ProductId, Segment, Category, Description, ProductPrice FROM Product ORDER BY ProductId DESC");
                }

            });

            return Ok(cacheEntry);
        }

        [HttpPost]
        public async Task<IActionResult> Buy([FromBody]Product product)
        {
            await _eventBus.Publish(product);
            return Ok();
        }
    }
}