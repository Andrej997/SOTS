using Microsoft.Extensions.DependencyInjection;
using Virtuoso.Interface;
using Virtuoso.Service;

namespace Virtuoso
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddVirtuoso(this IServiceCollection services)
        {
            services.AddTransient<IGraph, Graph>();

            return services;
        }
    }
}
