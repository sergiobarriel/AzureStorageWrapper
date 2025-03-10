using AzureStorageWrapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using samples;


AzuriteHelper.Start();// Starts the Azurite process by executing the run-azurite.bat file

var host = Host.CreateDefaultBuilder(args)
               .ConfigureAppConfiguration((context, config) =>
               {
                   config.AddJsonFile("app.settings.json", optional: false, reloadOnChange: true);// Add app.settings.json to the configuration

               })
               .ConfigureServices((context, services) => {
                   var connectionString = context.Configuration["StorageWrapper_ConnectionString"];// Set the StorageWrapper_ConnectionString string in the environment variables
                   services.AddAzureStorageWrapper(options =>
                   {
                       options.ConnectionString = connectionString;
                       options.MaxSasUriExpiration = 600;
                       options.DefaultSasUriExpiration = 300;
                       options.CreateContainerIfNotExists = true;
                   });//Configuration AzureStorageWrapper

                   services.AddExample_All();//Configuration Example_All

               })
               .Build();

var example_All = host.Services.GetService<Samples_All>();
await example_All.RunAsync();// Run the example_All service

Console.WriteLine("Press any key to exit");
Console.ReadKey();

