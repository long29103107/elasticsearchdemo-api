using BLogicLogging.DependencyInjection.Extensions;
using BLogicLogging.Middlewares;
using ElasticsearchDemo.Api.Endpoints;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
builder.Services.AddBLogicLogging();
builder.Host.AddBLogicLogging(builder.Configuration);

var app = builder.Build();
app.UseMiddleware<SerilogMiddleware>();
app.UseSerilogRequestLogging();
app.MapOpenApi();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/openapi/v1.json", "v1");
});

app.MapHomeRoutes();
app.Run();