using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.Elasticsearch;

namespace BLogicLogging.DependencyInjection.Extensions;

public static class LoggingHostExtensions
{
    public static void AddBLogicLogging(this IHostBuilder hostBuilder)
    {
        hostBuilder.UseSerilog((ctx, services, config) =>
        {
            config
                .ReadFrom.Configuration(ctx.Configuration)
                .ReadFrom.Services(services);
        });
    }
}