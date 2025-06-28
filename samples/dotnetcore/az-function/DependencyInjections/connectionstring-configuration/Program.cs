using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Hosting;
using AzureStorageWrapper;
using samples;

var builder = FunctionsApplication.CreateBuilder(args)
                                  .ConfigureFunctionsWebApplication();
builder.Services.AddExample_All();//Configuration Example_All

var configurationString = builder.Configuration["AzureStorageWrapper_ConnectionString"];
builder.Services.AddAzureStorageWrapper(configurationString);//Configuration AzureStorageWrapper

builder.Build().Run();
