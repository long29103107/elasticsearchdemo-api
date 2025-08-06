using BLogicLogging.DependencyInjection.Extensions;
using ElasticsearchDemo.Api.Endpoints;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
builder.Host.AddBLogicLogging(builder.Configuration);

var app = builder.Build();
app.UseSerilogRequestLogging();
app.MapOpenApi();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/openapi/v1.json", "v1");
});

app.UseHttpsRedirection();
app.MapHomeRoutes();
app.Run();