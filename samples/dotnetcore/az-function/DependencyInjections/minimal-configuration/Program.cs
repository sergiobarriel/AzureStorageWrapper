using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Hosting;
using AzureStorageWrapper;
using samples;

var builder = FunctionsApplication.CreateBuilder(args)
                                  .ConfigureFunctionsWebApplication();
builder.Services.AddExample_All();//Configuration Example_All

builder.Services.AddAzureStorageWrapper();//Configuration AzureStorageWrapper

builder.Build().Run();
