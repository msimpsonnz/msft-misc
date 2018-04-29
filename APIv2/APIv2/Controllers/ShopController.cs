using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Text;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Caching.Memory;

using APIv2.Models;
using APIv2.Services;

namespace APIv2.Controllers
{
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
