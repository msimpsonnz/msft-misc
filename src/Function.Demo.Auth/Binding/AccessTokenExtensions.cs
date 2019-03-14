//Credit https://github.com/BenMorris/FunctionsCustomSercuity
using System;
using Microsoft.Azure.WebJobs;

/// <summary>
/// Called from Startup to load the custom binding when the Azure Functions host starts up.
/// </summary>
namespace Function.Demo.Auth
{
    public static class AccessTokenExtensions
    {
        public static IWebJobsBuilder AddAccessTokenBinding(this IWebJobsBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.AddExtension<AccessTokenExtensionProvider>();
            return builder;
        }
    }
}