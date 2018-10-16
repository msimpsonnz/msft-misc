using MediaServices.Demo.Function;
using Microsoft.Extensions.Configuration;

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
