using Library;
using Microsoft.Extensions.DependencyInjection;

namespace WebApiSample.Infrastructure
{
    public static class ServiceCollectionExtension
    {
        public static void AddCounter(this IServiceCollection sc)
        {
            sc.AddSingleton<ICounterFactory, CounterFactory>();
            sc.AddSingleton<IValueRepository, ValueRepository>();
        }
    }
}