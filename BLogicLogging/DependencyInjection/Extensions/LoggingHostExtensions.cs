using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.Elasticsearch;

namespace BLogicLogging.DependencyInjection.Extensions;

public static class LoggingHostExtensions
{
    public static void AddBLogicLogging(this IHostBuilder hostBuilder, IConfiguration configuration)
    {   
        // hostBuilder.UseSerilog((ctx, lc) =>
        //     lc.ReadFrom.Configuration(ctx.Configuration));
        hostBuilder.UseSerilog((ctx, lc) => lc
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .ReadFrom.Configuration(ctx.Configuration)
            .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(ctx.Configuration["Serilog:WriteTo:1:Args:nodeUris"]))
            {
                AutoRegisterTemplate = true,
                IndexFormat = ctx.Configuration["Serilog:WriteTo:1:Args:indexFormat"],
                ModifyConnectionSettings = x =>
                    x.BasicAuthentication(
                        ctx.Configuration["Serilog:WriteTo:1:Args:basicAuthentication:username"], 
                        ctx.Configuration["Serilog:WriteTo:1:Args:basicAuthentication:password"])
            }));
    }
}