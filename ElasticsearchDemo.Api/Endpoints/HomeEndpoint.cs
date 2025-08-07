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
        endpoints.MapPost("/home", (ILogger _logger, HomeCreateRequest request) =>
        {
            _logger.Information("Payload request @request", request);
        });
        return endpoints;
    }
}

public sealed class HomeCreateRequest
{
    public string Value { get; set; }
}