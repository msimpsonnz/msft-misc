<<<<<<< HEAD
﻿using Microsoft.Extensions.Configuration;
using MediaServices.Demo.Function.Models;
=======
﻿using MediaServices.Demo.Function;
using Microsoft.Extensions.Configuration;
>>>>>>> 3798f69cf0aa01560cf0a1f0eb2bd4ce5d6f7293

namespace MediaServices.Demo.Tests
{
    public class TestHelper
    {
        public static IConfigurationRoot GetIConfigurationRoot()
        {
            return new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();
        }
        public static MetaData GetApplicationConfiguration()
        {
            var metaDataResult = new MetaData();
            var config = GetIConfigurationRoot();
            config.GetSection("result").Bind(metaDataResult);
            return metaDataResult;
        }
    }
}
