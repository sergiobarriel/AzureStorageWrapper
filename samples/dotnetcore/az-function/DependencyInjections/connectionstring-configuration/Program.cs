using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Hosting;
using AzureStorageWrapper;
using samples;

var builder = FunctionsApplication.CreateBuilder(args)
                                  .ConfigureFunctionsWebApplication();
builder.Services.AddExample_All();//Configuration Example_All

var configurationString = builder.Configuration["AzureStorageWrapper_ConnectionString"];
builder.Services.AddAzureStorageWrapper(configurationString);//Configuration AzureStorageWrapper

// The default container used for all storage operations if not specified explicitly. It is optional
//var defaultContainer = builder.Configuration["StorageWrapper_DefaultContainer"];
//builder.Services.AddAzureStorageWrapper(configurationString, defaultContainer);//Configuration AzureStorageWrapper


builder.Build().Run();
