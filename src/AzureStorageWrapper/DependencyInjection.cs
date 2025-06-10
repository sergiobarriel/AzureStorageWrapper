using System;
using Microsoft.Extensions.DependencyInjection;

namespace AzureStorageWrapper
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAzureStorageWrapper(this IServiceCollection serviceCollection)
            => AddAzureStorageWrapper(serviceCollection, Environment.GetEnvironmentVariable("StorageWrapper_ConnectionString"));

        public static IServiceCollection AddAzureStorageWrapper(this IServiceCollection serviceCollection, string connectionstring)
            => serviceCollection.AddAzureStorageWrapper(new AzureStorageWrapperOptions
            {
                ConnectionString = connectionstring,
                MaxSasUriExpiration = 600,
                DefaultSasUriExpiration = 300,
                CreateContainerIfNotExists = true
            });

        public static IServiceCollection AddAzureStorageWrapper(this IServiceCollection serviceCollection, AzureStorageWrapperOptions options)
            => serviceCollection.AddSingleton(options)
                                .AddSingleton<IAzureStorageWrapper, AzureStorageWrapper>()
              ;

        public static IServiceCollection AddAzureStorageWrapper(this IServiceCollection serviceCollection, Action<AzureStorageWrapperOptions> optionsAction)
        {
            var options = new AzureStorageWrapperOptions();

            optionsAction(options);

            return serviceCollection.AddSingleton(options)
                                    .AddSingleton<IAzureStorageWrapper, AzureStorageWrapper>();
        }
    }
}
