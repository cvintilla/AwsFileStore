using Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Services;
using Services.Abstract;

namespace AwsFileStore.Infrastructure
{
    public static class ConfigureServicesExtension
    {
        public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<UploadFileConfiguration>(configuration.GetSection("UploadFileConfiguration"));

            services.Configure<AwsS3Configuration>(configuration.GetSection("AwsS3Configuration"));

            services.AddTransient<IFileService, FileService>();

            services.AddTransient<IAwsService, AwsService>();

            services.AddHostedService();
        }
    }
}
