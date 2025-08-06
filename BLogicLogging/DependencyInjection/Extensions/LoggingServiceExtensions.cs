using BLogicLogging.Middlewares;
using Microsoft.Extensions.DependencyInjection;

namespace BLogicLogging.DependencyInjection.Extensions;

public static class LoggingServiceExtensions
{
    public static IServiceCollection AddBLogicLogging(this IServiceCollection services)
    {
        services.AddScoped<SerilogMiddleware>();
        
        return services;
    }
}