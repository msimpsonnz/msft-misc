using Blazor.FileReader;
using Microsoft.AspNetCore.Blazor.Builder;
using Microsoft.Extensions.DependencyInjection;
using Portal.Shared;
using Blazor.Extensions.Logging;

namespace Portal.Client
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(builder => builder.AddBrowserConsole());
            services.AddSingleton<IFileReaderService>(sp => new FileReaderService());
            services.AddSingleton<IUser, UserService>();
            services.AddSingleton<ISubmission, SubmissionService>();
        }

        public void Configure(IBlazorApplicationBuilder app)
        {
            app.AddComponent<App>("app");
        }
    }
}
