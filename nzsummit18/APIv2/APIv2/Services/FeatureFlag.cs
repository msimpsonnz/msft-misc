using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIv2.Services
{
    public class FeatureFlag : IFeatureFlag
    {
        private readonly IConfiguration _configuration;
        public FeatureFlag(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public bool IsFeatureEnabled(string feature)
        {
            var featureValue = _configuration[$"Features:{feature}"];
            if (string.IsNullOrWhiteSpace(featureValue))
            {
                throw new KeyNotFoundException(feature);
            }

            return bool.Parse(featureValue);
        }
    }
}
