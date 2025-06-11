using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Hosting;
using AzureStorageWrapper;
using samples;

var builder = FunctionsApplication.CreateBuilder(args)
                                  .ConfigureFunctionsWebApplication();

builder.Build().Run();
