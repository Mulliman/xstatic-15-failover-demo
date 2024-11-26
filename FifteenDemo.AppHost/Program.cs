using FifteenDemo.AppHost;

var builder = DistributedApplication.CreateBuilder(args);

var umbraco = builder.AddProject<Projects.FifteenDemo_Umbraco>("umbraco");
var staticSite = builder.AddProject<Projects.FifteenDemo_StaticSite>("staticsite");

builder.AddAzureFunctionsProject<Projects.FifteenDemo_AzureFunctions>("azurefunctions")
    .WithEnvironment("Cms.BaseUri", umbraco.GetEndpoint("https"))
    .WithEnvironment("Cms.ClientId", "umbraco-back-office-demo")
    .WithEnvironment("Cms.ClientSecret", "1234567890");

var yarp = builder.AddYarp("yarp")
    .WithHttpEndpoint(9999)
    .WithReference(umbraco)
    .WithReference(staticSite)
    .LoadFromConfiguration("ReverseProxy");

builder.Build().Run();