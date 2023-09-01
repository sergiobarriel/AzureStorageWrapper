using AzureStorageWrapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AzureStorageWrapper.Tests
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddAzureStorageWrapper(new AzureStorageWrapperConfiguration(GetConnectionString(), 360, 360));
        }

        private string GetConnectionString()
        {
            var fromEnvironmentVariable = GetConnectionStringFromEnvironmentVariable();

            var fromUserSecrets = GetConnectionStringFromUserSecretsFile();

            return !string.IsNullOrEmpty(fromEnvironmentVariable) 
                ? fromEnvironmentVariable
                : fromUserSecrets;
        }

        private string GetConnectionStringFromEnvironmentVariable() => Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING");
    
        private string GetConnectionStringFromUserSecretsFile()
        {
            var configuration = new ConfigurationBuilder()
                .AddUserSecrets<Startup>()
                .Build();

            var connectionString = configuration["AZURE_STORAGE_CONNECTION_STRING"];

            return connectionString;
        }
    }
}
