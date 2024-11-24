using FifteenDemo.AppHost;

var builder = DistributedApplication.CreateBuilder(args);

var umbraco = builder.AddProject<Projects.FifteenDemo_Umbraco>("umbraco");
var staticSite = builder.AddProject<Projects.FifteenDemo_StaticSite>("staticsite");

builder.AddYarp("yarp")
    .WithHttpEndpoint(9999)
    .WithReference(umbraco)
    .WithReference(staticSite)
    .LoadFromConfiguration("ReverseProxy");

builder.Build().Run();
