using Microsoft.Extensions.DependencyInjection;

namespace AzureStorageWrapper
{
    public static class DependencyInjectionExtension
    {
        public static void AddAzureStorageWrapper(this IServiceCollection serviceCollection, AzureStorageWrapperConfiguration configuration)
        {
            configuration.Validate();

            serviceCollection.AddSingleton(configuration);

            serviceCollection.AddSingleton<IAzureStorageWrapper, AzureStorageWrapper>();
        }
    }
}
