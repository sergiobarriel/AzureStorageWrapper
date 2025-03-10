using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AzureStorageWrapper.Tests.Should.DependencyInjection;

public class DependencyInjectionTests
{
    [Fact]
    public void AddAzureStorageWrapper_NoParameters_ShouldAddServices()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();
        Environment.SetEnvironmentVariable("StorageWrapper_ConnectionString", "UseDevelopmentStorage=true");

        // Act
        serviceCollection.AddAzureStorageWrapper();

        // Assert
        var serviceProvider = serviceCollection.BuildServiceProvider();
        var options = serviceProvider.GetService<AzureStorageWrapperOptions>();
        var wrapper = serviceProvider.GetService<IAzureStorageWrapper>();

        Assert.NotNull(options);
        Assert.NotNull(wrapper);
    }

    [Fact]
    public void AddAzureStorageWrapper_WithConnectionString_ShouldAddServices()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();
        var connectionString = "UseDevelopmentStorage=true";

        // Act
        serviceCollection.AddAzureStorageWrapper(connectionString);

        // Assert
        var serviceProvider = serviceCollection.BuildServiceProvider();
        var options = serviceProvider.GetService<AzureStorageWrapperOptions>();
        var wrapper = serviceProvider.GetService<IAzureStorageWrapper>();

        Assert.NotNull(options);
        Assert.Equal(connectionString, options.ConnectionString);
        Assert.NotNull(wrapper);
    }

    [Fact]
    public void AddAzureStorageWrapper_WithOptions_ShouldAddServices()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();
        var options = new AzureStorageWrapperOptions
        {
            ConnectionString = "UseDevelopmentStorage=true",
            MaxSasUriExpiration = 600,
            DefaultSasUriExpiration = 300,
            CreateContainerIfNotExists = true
        };

        // Act
        serviceCollection.AddAzureStorageWrapper(options);

        // Assert
        var serviceProvider = serviceCollection.BuildServiceProvider();
        var resolvedOptions = serviceProvider.GetService<AzureStorageWrapperOptions>();
        var wrapper = serviceProvider.GetService<IAzureStorageWrapper>();

        Assert.NotNull(resolvedOptions);
        Assert.Equal(options.ConnectionString, resolvedOptions.ConnectionString);
        Assert.Equal(options.MaxSasUriExpiration, resolvedOptions.MaxSasUriExpiration);
        Assert.Equal(options.DefaultSasUriExpiration, resolvedOptions.DefaultSasUriExpiration);
        Assert.Equal(options.CreateContainerIfNotExists, resolvedOptions.CreateContainerIfNotExists);
        Assert.NotNull(wrapper);
    }

    [Fact]
    public void AddAzureStorageWrapper_WithOptionsAction_ShouldAddServices()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();

        // Act
        serviceCollection.AddAzureStorageWrapper(options =>
        {
            options.ConnectionString = "UseDevelopmentStorage=true";
            options.MaxSasUriExpiration = 600;
            options.DefaultSasUriExpiration = 300;
            options.CreateContainerIfNotExists = true;
        });

        // Assert
        var serviceProvider = serviceCollection.BuildServiceProvider();
        var options = serviceProvider.GetService<AzureStorageWrapperOptions>();
        var wrapper = serviceProvider.GetService<IAzureStorageWrapper>();

        Assert.NotNull(options);
        Assert.Equal("UseDevelopmentStorage=true", options.ConnectionString);
        Assert.Equal(600, options.MaxSasUriExpiration);
        Assert.Equal(300, options.DefaultSasUriExpiration);
        Assert.True(options.CreateContainerIfNotExists);
        Assert.NotNull(wrapper);
    }
}
