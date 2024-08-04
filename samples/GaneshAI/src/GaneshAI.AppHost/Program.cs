IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);
IResourceBuilder<RedisResource> cache = builder.AddRedis("cache");
IResourceBuilder<ProjectResource> apiService = builder.AddProject<Projects.GaneshAI_ApiService>("apiservice");
builder.AddProject<Projects.GaneshAI_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(cache)
    .WithReference(apiService);
builder.Build().Run();
