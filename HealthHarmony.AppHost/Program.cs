var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.HealthHarmony>("healthharmony");

builder.Build().Run();
