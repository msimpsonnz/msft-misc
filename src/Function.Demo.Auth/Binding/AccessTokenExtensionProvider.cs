//Credit https://github.com/BenMorris/FunctionsCustomSercuity
using Microsoft.Azure.WebJobs.Host.Config;

/// <summary>
/// Wires up the attribute to the custom binding.
/// </summary>
namespace Function.Demo.Auth
{
    public class AccessTokenExtensionProvider : IExtensionConfigProvider
    {
        public void Initialize(ExtensionConfigContext context)
        {
            // Creates a rule that links the attribute to the binding
            var provider = new AccessTokenBindingProvider();
            var rule = context.AddBindingRule<AccessTokenAttribute>().Bind(provider);
        }
    }
}