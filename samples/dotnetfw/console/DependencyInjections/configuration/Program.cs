using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Threading.Tasks;
using AzureStorageWrapper;
using Microsoft.Extensions.DependencyInjection;

namespace samples
{
    class Program
    {
        static async Task Main(string[] args)
        {
            AzuriteHelper.Start();// Starts the Azurite process by executing the run-azurite.bat file
            var serviceProvider = ConfigureServices((configuration, services) =>
            {
                var connectionString = configuration["StorageWrapper_ConnectionString"];// Set the StorageWrapper_ConnectionString string in the environment variables
                services.AddAzureStorageWrapper(options =>
                {
                    options.ConnectionString = connectionString;
                    options.MaxSasUriExpiration = 600;
                    options.DefaultSasUriExpiration = 300;
                    options.CreateContainerIfNotExists = true;
                });//Configuration AzureStorageWrapper

                services.AddExample_All();//Configuration Example_All

            });

            var example_All = serviceProvider.GetService<Samples_All>();
            await example_All.RunAsync();// Run the example_All service

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

        private static ServiceProvider ConfigureServices(Action<NameValueCollection, ServiceCollection> configurateDelegate)
        {
            var services = new ServiceCollection();
            configurateDelegate(ConfigurationManager.AppSettings, services);
            // Construir el proveedor de servicios
            return services.BuildServiceProvider();
        }
    }
}
