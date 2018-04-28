namespace Blazor.Retail.Server.Services
{
    public interface IFeatureFlag
    {
        bool IsFeatureEnabled(string feature);
    }
}