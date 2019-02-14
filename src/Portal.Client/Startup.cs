using Blazor.FileReader;
using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;
using Portal.Shared;

namespace Portal.Client
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IFileReaderService>(sp => new FileReaderService());
            services.AddSingleton<IUser, UserService>();
            services.AddSingleton<ISubmission, SubmissionService>();
        }

        public void Configure(IComponentsApplicationBuilder app)
        {
            app.AddComponent<App>("app");
        }
    }
}
