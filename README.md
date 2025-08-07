# Blogic Logging
This guide helps you set up centralized logging for .NET Core applications using Serilog, Elasticsearch, and Kibana.

## 🔧 1. Use library
### Add reference to project ``BLogicLogging.csproj``
## 🔧 2. Add host and service in ``Program.cs``
```
//Service
builder.Host.AddBLogicLogging();
builder.Services.AddBLogicLogging();

//Pipeline
app.UseMiddleware<SerilogMiddleware>();
```
## 🔧 3. Add configuration to ``appsettings.json``
```
  "Serilog": {
    "Using": [ "Serilog.Sinks.Console", "Serilog.Sinks.Elasticsearch" ],
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Warning",
        "System": "Warning",
        "Microsoft.Hosting.Lifetime": "Information"
      }
    },
    "Enrich": [
      "FromLogContext",
      "WithMachineName",
      "WithProcessId",
      "WithThreadId",
      "WithEnvironmentName"
    ],
    "WriteTo": [
      {
        "Name": "Console"
      },
      {
        "Name": "Elasticsearch",
        "Args": {
          "nodeUris": "http://localhost:9200",
          "indexFormat": "logstash-elasticsearchdemo-api-development-2025-08", 
          "autoRegisterTemplate": true
        }
      }
    ],
    "Properties": {
      "Application": "ElasticsearchDemo.Api"
    }
  }
```

