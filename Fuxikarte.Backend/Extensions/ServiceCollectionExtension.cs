using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Fuxikarte.Backend.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddServicesFromNamespace(this IServiceCollection services, Assembly assembly, string @namespace)
        {
            var serviceTypes = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.Namespace == @namespace && t.Name.EndsWith("Service"));

            foreach (var type in serviceTypes)
            {
                services.AddScoped(type); // ou Transient / Singleton se preferir
            }
        }
    }
}
