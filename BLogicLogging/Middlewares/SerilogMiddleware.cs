using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Serilog;
using Serilog.Context;

namespace BLogicLogging.Middlewares;

public class SerilogMiddleware(IWebHostEnvironment env, ILogger logger) : IMiddleware
{
    private const string HeaderKey = "request-correlation-id";
    private readonly IWebHostEnvironment _env = env;
    private readonly ILogger _logger = logger;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        context.Request.EnableBuffering(); 
        string body = string.Empty;
        if (context.Request.ContentLength > 0 && context.Request.Body.CanRead)
        {
            using var reader = new StreamReader(
                context.Request.Body,
                encoding: Encoding.UTF8,
                detectEncodingFromByteOrderMarks: false,
                leaveOpen: true);
        
            body = await reader.ReadToEndAsync();
            context.Request.Body.Position = 0; 
        }
        
        var correlationId = context.Request.Headers.ContainsKey(HeaderKey)
            ? context.Request.Headers[HeaderKey].ToString()
            : Guid.NewGuid().ToString();

        using (LogContext.PushProperty("RCID", correlationId))
        using (LogContext.PushProperty("ApplicationName", env.ApplicationName))
        using (LogContext.PushProperty("Environment", _env.EnvironmentName))
        {   
            _logger.Information("Request {method} {url} Body: {body}",
                context.Request.Method,
                context.Request.Path,
                body);
            await next.Invoke(context);
        } 
    }
}