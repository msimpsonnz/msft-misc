using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Retail.Common.Models;
using System.Text;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Caching.Memory;
using Retail.Web.Services;

namespace Retail.Web.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/shop")]
    public class ShopControllerV1 : Controller
    {
        private static HttpClient httpClient = new HttpClient();
        private readonly IConfiguration _configuration;
        private readonly IFeatureFlag _feature;
        private static int failCounter;

        public ShopControllerV1(IConfiguration configuration, IFeatureFlag feature)
        {
            _configuration = configuration;
            _feature = feature;
        }

        [HttpGet]
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
            using (IDbConnection dbConnection = new SqlConnection(_configuration["Sql"]))
            {
                dbConnection.Open();
                return Ok(dbConnection.Query<Product>("SELECT TOP 10 ProductId, Segment, Category, Description, ProductPrice FROM Product ORDER BY ProductId DESC"));
            }
        }

        [HttpPost]
        public IActionResult Buy([FromBody]Product product)
        {
            SalesLine sale = new SalesLine(
                DateTime.UtcNow,
                1,
                1,
                1,
                product.ProductId,
                product.ProductPrice,
                1
                );
            using (IDbConnection dbConnection = new SqlConnection(_configuration["Sql"]))
            {
                string sQuery = "INSERT INTO SalesLines (SalesDate, LocationId, PosId, EmployeeId, BasketId, ProductId, ProductPrice, ProductUnits)"
                                + " VALUES(@SalesDate, @LocationId, @PosId, @EmployeeId, @BasketId, @ProductId, @ProductPrice, @ProductUnits)";
                dbConnection.Open();
                dbConnection.Execute(sQuery, sale);
            }
            return Ok();
        }
    }
    [ApiVersion("2.0")]
    [Route("api/shop")]
    public class ShopControllerV2 : Controller
    {
        private static HttpClient httpClient = new HttpClient();
        private readonly IConfiguration _configuration;
        private IMemoryCache _cache;
        private IEventBus _eventBus;

        public ShopControllerV2(IConfiguration configuration, IMemoryCache cache, IEventBus eventBus)
        {
            _configuration = configuration;
            _cache = cache;
            _eventBus = eventBus;
        }

        [HttpGet]
        public IActionResult GetAllProducts()
        {
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
