using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Hosting;
using AzureStorageWrapper;
using samples;

var builder = FunctionsApplication.CreateBuilder(args)
                                  .ConfigureFunctionsWebApplication();
builder.Services.AddExample_All();//Configuration Example_All

var connectionString = builder.Configuration["AzureStorageWrapper_ConnectionString"];
var defaultContainer = builder.Configuration["StorageWrapper_DefaultContainer"];
builder.Services.AddAzureStorageWrapper(options =>
{
    options.ConnectionString = connectionString;
    options.MaxSasUriExpiration = 600;
    options.DefaultSasUriExpiration = 300;
    options.CreateContainerIfNotExists = true;
    //options.DefaultContainer= defaultContainer; // The default container used for all storage operations if not specified explicitly. It is optional
});//Configuration AzureStorageWrapper

builder.Build().Run();
