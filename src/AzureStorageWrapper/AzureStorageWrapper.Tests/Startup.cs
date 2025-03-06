using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AzureStorageWrapper.Tests
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddAzureStorageWrapper(configuration =>
            {
                configuration.ConnectionString = "UseDevelopmentStorage=true";
                configuration.MaxSasUriExpiration = 360;
                configuration.DefaultSasUriExpiration = 360;
                configuration.CreateContainerIfNotExists = true;
            });
        }
    }
}
