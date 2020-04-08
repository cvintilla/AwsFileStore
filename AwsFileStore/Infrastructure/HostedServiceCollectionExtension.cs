using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwsFileStore.Infrastructure
{
    public static class HostedServiceCollectionExtension
    {
        public static IServiceCollection AddHostedService(this IServiceCollection services)
        {
            services.AddHostedService<S3ManagerHostedService>();

            return services;
        }
    }
}
