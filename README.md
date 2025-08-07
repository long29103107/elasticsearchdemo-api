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
          "indexFormat": "logstash-elasticsearchdemo-api-development-{0:yyyy-MM}",
          "autoRegisterTemplate": true,
          "basicAuthentication": {
            "username": "elastic",
            "password": "adminpass"
          }
        }
      }
    ],
  }
}
```


# Setup Elasticsearch + Kibana
## 1. Create file ``docker-compose.yml``
```
version: '3.8'

services:
  elasticsearch:
    image: docker.elastic.co/elasticsearch/elasticsearch:8.13.0
    container_name: elasticsearch
    environment:
      - node.name=elasticsearch
      - discovery.type=single-node
      - xpack.security.enabled=true
      - ELASTIC_PASSWORD=adminpass
      - xpack.security.authc.api_key.enabled=true
    volumes:
      - esdata:/usr/share/elasticsearch/data
    ports:
      - 9200:9200
    healthcheck:
      test: curl --silent --fail http://localhost:9200 || exit 1
      interval: 10s
      retries: 5
    networks:
      - elk

  kibana:
    image: docker.elastic.co/kibana/kibana:8.13.0
    container_name: kibana
    depends_on:
      - elasticsearch
    environment:
      - ELASTICSEARCH_HOSTS=http://elasticsearch:9200
      - ELASTICSEARCH_SERVICEACCOUNTTOKEN={{GENERATED_TOKEN}}
    ports:
      - 5601:5601
    networks:
      - elk

volumes:
  esdata:

networks:
  elk:
    driver: bridge
```
## 2. Run container elasticsearch
```
docker compose up elasticsearch 
```

## 3. Create get elasticsearch
```
docker exec -it elasticsearch bin/elasticsearch-service-tokens create elastic/kibana kibana-token
```
## 4. Replace new token to ``{{GENERATED_TOKEN}}`` in file ``docker-compose.yml``

```
      - ELASTICSEARCH_SERVICEACCOUNTTOKEN=181bf9c497d190b1acee26eeca4742cbe1737e006fa82de3cc1d0a23125423cf
```

## 5. Run container kibana
```
docker compose up kibana
```

## 6. Access URL
- Elasticsearch: http://localhost:9200/
- Kibana: http://localhost:5601/

## 7. Login on kibana UI
![alt text](http://url/to/img.png)

## 8. View log
``Stack Management`` > ``Index Management``
Select index maps with pattern of indexFormat in ``appsettings.json``
Example:
```
indexFormat: logstash-elasticsearchdemo-api-development-{0:yyyy-MM}
pattern: logstash-elasticsearchdemo-api-development-*
```

## 9. Trace log by RCID
Select log need to view > ``Discorver index`` > ``Create data view``
1. Enter name 
2. Enter pattern
3. Save changes
4. Enter on filter ``fields.RCID = '3a4121b5-9811-4fa2-9825-4ea562c7ec8d'``