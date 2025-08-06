using Serilog;
using ILogger = Serilog.ILogger;

namespace ElasticsearchDemo.Api.Endpoints;

public static class HomeEndpointExtensions
{
    public static IEndpointRouteBuilder MapHomeRoutes(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/home", (ILogger _logger) =>
        {
            _logger.Information("Hello World!");
            
        });
        return endpoints;
    }
}