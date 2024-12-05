using HouseRentingSystem.Services.Data;
using HouseRentingSystem.Services.Data.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace HouseRentingSystem.Web.Infrastructure.Extensions
{
    public  static class WebApplicationBuilderExtensions
    {
        /// <summary>
        /// This method registers all services with their interface and implementations of given assembly.
        /// The assembly is taken from the type of random service implementation provided.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="serviceType">Type of random implementation.</param>
        /// <exception cref="InvalidOperationException"></exception>
        public static void AddApplicationServices(this IServiceCollection services, Type serviceType) 
        {
            Assembly? serviceAssembly = Assembly.GetAssembly(serviceType);
            if (serviceAssembly == null) 
            {
                throw new InvalidOperationException("Invalid service type provided!");
            }

            Type[] serviceTypes = serviceAssembly.GetTypes()
                .Where(t => t.Name.EndsWith("Service") && !t.IsInterface)
                .ToArray();

            foreach (Type type in serviceTypes)
            {
                Type interfaceType = type.GetInterface($"I{type.Name}");
                if (interfaceType == null) 
                {
                    throw new InvalidOperationException($"No interface is provided for the service with name:{type.Name}");
                }
                services.AddScoped(interfaceType, type);
            }
     
        }
    }
}
