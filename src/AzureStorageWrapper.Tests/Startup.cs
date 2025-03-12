using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AzureStorageWrapper.Tests
{
    public class Startup
    {
        public const string DefaultContainer = "default-files";
        public void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddAzureStorageWrapper(options =>
            {
                options.ConnectionString = "UseDevelopmentStorage=true";
                options.MaxSasUriExpiration = 360;
                options.DefaultSasUriExpiration = 360;
                options.CreateContainerIfNotExists = true;
                options.DefaultContainer = DefaultContainer;
            });
        }
    }
}
