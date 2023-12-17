using System;
using Microsoft.Extensions.DependencyInjection;

namespace AzureStorageWrapper
{
    public static class DependencyInjectionExtension
    {
        public static void AddAzureStorageWrapper(this IServiceCollection serviceCollection, AzureStorageWrapperConfiguration configuration)
        {
            //configuration.Validate();
            
            serviceCollection.AddSingleton<IUriService, UriService>();

            serviceCollection.AddSingleton(configuration);
            
            serviceCollection.AddSingleton<IAzureStorageWrapper, AzureStorageWrapper>();
        }

        public static void AddAzureStorageWrapper(this IServiceCollection serviceCollection, Action<AzureStorageWrapperConfiguration> configurationAction)
        {
            var configuration = new AzureStorageWrapperConfiguration();

            configurationAction(configuration);
            
            //configuration.Validate();

            serviceCollection.AddSingleton<IUriService, UriService>();

            serviceCollection.AddSingleton(configuration);
            
            serviceCollection.AddSingleton<IAzureStorageWrapper, AzureStorageWrapper>();
        }
    }
}
