var builder = DistributedApplication.CreateBuilder(args);
var redis = builder.AddRedis("redis", 6379)
    .WithPersistence()
    .WithDataVolume();
var apiService = builder.AddProject<Projects.GaneshAI_ApiService>("apiservice");
builder.AddProject<Projects.GaneshAI_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(redis)
    .WithReference(apiService);
builder.Build().Run();
